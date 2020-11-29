using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class GoldNumText : MonoBehaviour
{
	private Text goldText;

	private void Awake()
	{
		goldText = GetComponent<Text>();
	}
	private void FixedUpdate ()
	{
		goldText.text = PlayerInventory.gold.ToString();
	}
}
