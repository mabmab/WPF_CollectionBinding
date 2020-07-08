using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace CollectionBinding
{
    ///++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// スタッフ情報構造体
    /// </summary>
    ///++++++++++++++++++++++++++++++++++++++++++++++++++++
    public struct StaffInfoArgs
    {
        /// <summary>番号</summary>
        public uint Number { get; set; }
        /// <summary>氏名</summary>
        public string Name { get; set; }

        /// <summary>コンストラクター</summary>
        /// <param name="number">番号</param>
        /// <param name="name">氏名</param>
        public StaffInfoArgs(uint number, string name)
        {
            Number = number;
            Name = name;
        }
    }
    
    ///++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// ビューモデルクラス
    /// </summary>
    ///++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class ViewModel : DependencyObject
    {
        //=================================================
        // 依存関係プロパティ/CLRプロパティ
        //=================================================

        /// <summary>スタッフ情報コレクションの依存関係プロパティ</summary>
        public static readonly DependencyProperty StaffInfosProperty =
            DependencyProperty.Register(
                "StaffInfos",
                typeof(ObservableCollection<StaffInfoArgs>),
                typeof(ViewModel),
                new PropertyMetadata(new ObservableCollection<StaffInfoArgs>()
            )
        );

        /// <summary>スタッフ情報コレクションのCLRプロパティ</summary>
        public ObservableCollection<StaffInfoArgs> StaffInfos
        {
            get { return (ObservableCollection<StaffInfoArgs>)GetValue(StaffInfosProperty); }
            set { SetValue(StaffInfosProperty, value); }
        }
    }
}
