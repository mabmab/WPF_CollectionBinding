using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace CollectionBinding
{
    ///++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// メインウィンドウクラス
    /// </summary>
    ///++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class MainWindow : Window
    {
        //=================================================
        // コンストラクター
        //=================================================

        #region コンストラクター
        /// <summary>デフォルトコンストラクター/summary>
        public MainWindow()
        {
            InitializeComponent();
        }
        #endregion

        //=================================================
        // イベントハンドラー
        //=================================================

        /// <summary>追加メニュークリックイベント</summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void MenuItem_Add_Click(object sender, RoutedEventArgs args)
        {
            // 空のスタッフ情報をコレクションに追加
            var info = new StaffInfoArgs(1, "Hoge");
            _vm.StaffInfos.Add(info);
        }

        /// <summary>削除メニュークリックイベント</summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void MenuItem_Delete_Click(object sender, RoutedEventArgs args)
        {
            // スタッフ情報コレクションが空なら何もしない
            if (!_vm.StaffInfos.Any()) return;

            // コレクション最後のスタッフ情報を削除
            _vm.StaffInfos.RemoveAt(_vm.StaffInfos.Count - 1);
        }

        /// <summary>ソートメニュークリックイベント</summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void MenuItem_Sort_Click(object sender, RoutedEventArgs args)
        {
            // スタッフ情報コレクションが空なら何もしない
            if (!_vm.StaffInfos.Any()) return;

            // コレクションのソート
            _vm.StaffInfos = new Observable​Collection<StaffInfoArgs>(_vm.StaffInfos.OrderBy(info => info.Number));
        }
    }
}
