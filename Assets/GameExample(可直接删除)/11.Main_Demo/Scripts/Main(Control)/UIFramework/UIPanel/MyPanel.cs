//该脚本为自动生成的Panel脚本
using UnityEngine;
using UnityEngine.UI;

namespace MyDemo
{
	public class MyPanel : BasePanel
	{

		private MyPanelData MyPanelData;

		public Button Btn_test1;


		public Button Btn_test2;


		public Button Btn_test3;


		public Button Btn_test4;


		public Button Btn_test5;

		public Transform Img_tips;

		//1. 使用动画库前先声明对象并初始化.. 之后直接调用库内方法即可
		public UIAnimation anim = new UIAnimation();

		public override void Awake()
		{
			base.Awake();

			Btn_test1.onClick.AddListener(OnBtn_test1_OnClick);

			Btn_test2.onClick.AddListener(OnBtn_test2_OnClick);

			Btn_test3.onClick.AddListener(OnBtn_test3_OnClick);

			Btn_test4.onClick.AddListener(OnBtn_test4_OnClick);

			Btn_test5.onClick.AddListener(OnBtn_test5_OnClick);
		}

		public void OnDestroy()
		{

			Btn_test1.onClick.RemoveListener(OnBtn_test1_OnClick);

			Btn_test2.onClick.RemoveListener(OnBtn_test2_OnClick);

			Btn_test3.onClick.RemoveListener(OnBtn_test3_OnClick);

			Btn_test4.onClick.RemoveListener(OnBtn_test4_OnClick);

			Btn_test5.onClick.RemoveListener(OnBtn_test5_OnClick);
		}

		public override void OnEnter() { base.OnEnter(); }

		public override void OnPause() { base.OnPause(); }

		public override void OnResume() { base.OnResume(); }

		public override void OnExit() { base.OnExit(); }

		private void OnBtn_test1_OnClick()
		{
			if (!Img_tips.gameObject.activeInHierarchy)
			{
				anim.MoveIn_Chilid(Img_tips, new Vector2(450, 500));
			}
			else
			{
				anim.MoveOut_Chilid(Img_tips, new Vector2(450, 500));
			}

		}

		private void OnBtn_test2_OnClick()
		{
			if (!Img_tips.gameObject.activeInHierarchy)
			{
				anim.UpScale_Child(Img_tips);
			}
			else
			{
				anim.DownScale_Child(Img_tips);
			}
		}

		private void OnBtn_test3_OnClick()
		{
			if (!Img_tips.gameObject.activeInHierarchy)
			{
				anim.FadeIn_Child(Img_tips);
			}
			else
			{
				anim.FadeOut_Child(Img_tips);
			}
		}

		private void OnBtn_test4_OnClick()
		{ }

		private void OnBtn_test5_OnClick() { }


		public override void SetPanelData(UIPanelData uiPanelData)
		{
			MyPanelData = (MyPanelData)uiPanelData;
		}
	}

}
