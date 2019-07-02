namespace UI
{
    public interface IButtonInterface
    {
        //　Buttonを押した時の処理
        void OnClick();

        //　ボタンを選択されたときの処理
        void OnSelected();

        //　ボタンから移動ときの処理
        void OnDeselected();

        //　キャンセルを押したときなどに戻るボタンを選択状態にする処理
        void SelectReturnButton();
    }
}
