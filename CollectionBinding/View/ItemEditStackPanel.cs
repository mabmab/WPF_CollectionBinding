using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace CollectionBinding
{
    ///++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// アイテム編集スタックパネルクラス
    /// </summary>
    ///++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class ItemEditStackPanel : StackPanel
    {
        //=================================================
        // フィールド
        //=================================================

        /// <summary>ドラッグアイテム</summary>
        private ItemEditControl _draggedItem = null;
        /// <summary>ドラッグアイテムインデックス</summary>
        private int? _draggedItemIndex = null;

        /// <summary>初期位置</summary>
        private Point? _initialPosition = null;
        /// <summary>マウス位置とアイテム位置のオフセット</summary>
        private Point _mouseOffsetFromItem;

        /// <summary>ドラッグユーザーコントロール装飾オブジェクト</summary>
        private DragUserControlAdorner _dragContentAdorner = null;

        //=================================================
        // 依存関係プロパティ/CLRプロパティ
        //=================================================

        /// <summary>スタッフ情報コレクションの依存関係プロパティ</summary>
        public static readonly DependencyProperty StaffInfosProperty =
            ViewModel.StaffInfosProperty.AddOwner(
                typeof(ItemEditStackPanel),
                new PropertyMetadata(
                    null,
                    (d, e) =>
                    {
                        // スタッフ情報コレクション変更イベントハンドラーを設定する
                        var itemPanel = (d as ItemEditStackPanel);
                        itemPanel.StaffInfos.CollectionChanged += itemPanel.StaffInfoCollectionChanged;
                    }
                )
            );

        /// <summary>スタッフ情報コレクションのCLRプロパティ</summary>
        public ObservableCollection<StaffInfoArgs> StaffInfos
        {
            private get { return (ObservableCollection<StaffInfoArgs>)GetValue(StaffInfosProperty); }
            set { SetValue(StaffInfosProperty, value); }
        }


        /// <summary>デバッグ出力フラグ</summary>
        public bool IsDebugOUt { private get; set; } = false;

        //=================================================
        // コンストラクター
        //=================================================

        /// <summary>デフォルトコンストラクター</summary>
        public ItemEditStackPanel()
        {
            // イベントハンドラーの設定
            MouseLeftButtonDown += OnMouseLeftButtonDown;
            MouseMove += OnMouseMove;
            MouseLeftButtonUp += OnMouseLeftButtonUp;
            LostMouseCapture += OnLostMouseCapture;
        }

        //=================================================
        // イベントハンドラー(マウス操作)
        //=================================================

        /// <summary>マウス左ボタンダウンイベント</summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs args)
        {
            // マウス左ボタンダウン位置のアイテムがドラッグ対象外なら何もしない
            ItemEditControl draggedItem = args.Source as ItemEditControl;
            if (draggedItem == null) return;

            // マウスキャプチャーに成功した場合にドラッグを開始する
            if (CaptureMouse())
            {
                // ドラッグアイテムとそのインデックスの保存
                _draggedItem = draggedItem;
                _draggedItemIndex = Children.IndexOf(draggedItem);

                // 初期位置とマウス位置とアイテム位置のオフセットの保存
                _initialPosition = PointToScreen(args.GetPosition(this));
                _mouseOffsetFromItem = draggedItem.PointFromScreen(_initialPosition.Value);
            }
        }

        /// <summary>マウス移動イベント</summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnMouseMove(object sender, MouseEventArgs args)
        {
            // マウス左ボタンダウン前のマウス移動では何もしない
            if (_draggedItem == null) return;

            // ドラッグ装飾が未生成の場合はドラッグ装飾を生成する
            if (_dragContentAdorner == null)
            {
                _dragContentAdorner = new DragUserControlAdorner(this, _draggedItem, _mouseOffsetFromItem);
            }

            // マウス移動中のスクリーン座標位置を取得
            Point ptMoveScreen = PointToScreen(args.GetPosition(this));

            // ドラッグ装飾を移動
            _dragContentAdorner.SetScreenPosition(ptMoveScreen);
        }

        /// <summary>マウス左ボタンアップイベント</summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs args)
        {
            // 左ボタンアップのポイントのアイテムを取得
            Point ptUp = args.GetPosition(this);
            var dropTargetItem = GetChildElement(ptUp) as ItemEditControl;

            // ドラッグ中、かつ、ドロップ位置のアイテムがドロップ対象ならアイテムを移動する
            if (_draggedItem != null && dropTargetItem != null)
            {
                // ビューモデルの取得
                var vm = DataContext as ViewModel;

                // スタッフ情報の移動
                int dropTargetItemIndex = Children.IndexOf(dropTargetItem);
                StaffInfoArgs info = vm.StaffInfos[_draggedItemIndex.Value];
                vm.StaffInfos.Remove(info);
                vm.StaffInfos.Insert(dropTargetItemIndex, info);
            }

            // マウスキャプチャーのリリース
            ReleaseMouseCapture();
        }

        /// <summary>マウスキャプチャーロストイベント</summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnLostMouseCapture(object sender, MouseEventArgs args)
        {
            // 装飾のデタッチ
            _dragContentAdorner?.Detach();
            _dragContentAdorner = null;

            // フィールドの初期化
            _draggedItem = null;
            _draggedItemIndex = null;
        }

        //=================================================
        // イベントハンドラー(コレクション)
        //=================================================

        /// <summary>スタッフ情報コレクション変更イベント</summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void StaffInfoCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            // Bindingを解除して全削除
            foreach (var itemEdit in Children.OfType<ItemEditControl>())
            {
                BindingOperations.ClearAllBindings(itemEdit);
            }
            Children.Clear();

            // 新しいスタッフ情報コレクションからスタックパネルを再構築
            for (int i = 0; i < StaffInfos.Count; i++)
            {
                // スタックパネルにアイテム編集コントロールを追加
                var itemEdit = new ItemEditControl();
                itemEdit.IsDebugOUt = IsDebugOUt;
                Children.Add(itemEdit);

                // アイテム編集コントロールとスタッフ情報コレクションの要素ひとつをBinding
                Binding b = new Binding();
                b.Source = this;
                b.Path = new PropertyPath($"StaffInfos[{i}]");
                b.Mode = BindingMode.TwoWay;
                BindingOperations.SetBinding(itemEdit, ItemEditControl.StaffInfoProperty, b);
            }
        }

        //=================================================
        // プライベートメソッド
        //=================================================

        /// <summary>子要素の取得</summary>
        /// <param name="pt">位置</param>
        /// <returns></returns>
        private UIElement GetChildElement(Point pt)
        {
            UIElement hit = null;
            VisualTreeHelper.HitTest(
                this,
                (o) =>
                {
                    if (o.GetType() == typeof(ItemEditControl))
                    {
                        hit = o as UIElement;
                        return HitTestFilterBehavior.Stop;
                    }
                    return HitTestFilterBehavior.Continue;
                },
                (o) =>
                {
                    return HitTestResultBehavior.Stop;
                },
                new PointHitTestParameters(pt));

            // ヒットした子要素を返す
            return hit;
        }
    }
}
