using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{

	public float MaxHp = 100;
	public DamageSystem healthSystem;

	public RectTransform rect;

	
    void Update()
    {
		Vector2 size = new Vector2();
		size.x = 100 * (healthSystem.HP / MaxHp);
		size.y = 100;

		Vector2 pos = rect.localPosition;
		pos.x = 50 - 50 * (healthSystem.HP / MaxHp);
		rect.sizeDelta = size;
		rect.localPosition = pos;
	}
}
