using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace CollectionBinding
{
    ///++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// アイテム編集コントロールクラス
    /// </summary>
    ///++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class ItemEditControl : UserControl
    {
        //=================================================
        // 依存関係プロパティ/CLRプロパティ
        //=================================================

        /// <summary>スタッフ情報の依存関係プロパティ</summary>
        public static readonly DependencyProperty StaffInfoProperty =
            DependencyProperty.Register(
                "StaffInfo",
                typeof(StaffInfoArgs),
                typeof(ItemEditControl),
                new PropertyMetadata(
                    new StaffInfoArgs(0, "(初期値)"),
                    (d, e) =>
                    {
                        if (((ItemEditControl)d).IsDebugOUt)
                        {
                            var args = (StaffInfoArgs)e.NewValue;
                            Debug.WriteLine($"☆StaffInfoPropertyChanged[番号：{args.Number} 氏名：{args.Name}]");
                        }
                    }
                )
            );

        /// <summary>スタッフ情報のCLRプロパティ</summary>
        public StaffInfoArgs StaffInfo
        {
            get { return (StaffInfoArgs)GetValue(StaffInfoProperty); }
            set { SetValue(StaffInfoProperty, value); }
        }

        /// <summary>デバッグ出力フラグ</summary>
        public bool IsDebugOUt { private get; set; } = false;

        //=================================================
        // コンストラクター
        //=================================================

        /// <summary>デフォルトコンストラクター/summary>
        public ItemEditControl()
        {
            InitializeComponent();
        }

        //=================================================
        // イベントハンドラー
        //=================================================

        /// <summary>番号確定ボタンのクリックイベント</summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnNumberEnterButtonClick(object sender, RoutedEventArgs args)
        {
            int number;
            if (!Int32.TryParse(xNumber.Text, out number)) number = 0;
            StaffInfo = new StaffInfoArgs((uint)number, StaffInfo.Name);
        }

        /// <summary>名前確定ボタンのクリックイベント</summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnNameEnterButtonClick(object sender, RoutedEventArgs args)
        {
            StaffInfo = new StaffInfoArgs(StaffInfo.Number, xName.Text);
        }
    }
}
