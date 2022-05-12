using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

namespace Code
{
	public class BarController : Singleton<BarController>
	{
		[SerializeField] private Image fillImage;
		[SerializeField] private TextMeshProUGUI percentText;


		
		public void Display(float normalized)
		{
			fillImage.fillAmount = normalized;
			percentText.SetText($"%{(int)(normalized * 100)}");
		}
		
	}
}