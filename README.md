# WPF_CollectionBinding
WPFのコレクション要素をバインディングするサンプルコードです。
## 【目的】
1. コレクション（配列やリスト）をリストなどのUIに直接バインドする方法の確立
    - コレクションの子要素もバインディングする
2. StackPanelのItemのドラッグ＆ドロップによる入れ替え方法の確立
    - Itemのドラッグ＆ドロップによる入れ替え
    - ドラッグ＆ドロップ中のゴースト表示
3. StackPanelの子要素の動的な追加削除方法の確立
    - 動的な追加削除に伴い、Bindingも動的に行う
4. StackPanelの子要素のHitTest方法の確立

## 【課題】
1. FluidMoveBehaviorを使用しても、アニメーションしない
    - Childrenプロパティを直接操作していないから？
    - StackPanelの派生クラスだから？
2. StackPanel要素を入れ替える際にエラーが発生している
    - 一時的でもBinding要素が0になるのが問題らしい
