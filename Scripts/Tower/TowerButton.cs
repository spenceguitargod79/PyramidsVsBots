using UnityEngine;


public class TowerButton : MonoBehaviour {
[SerializeField]
private int towerPrice;

[SerializeField]
private Tower towerObject;
[SerializeField]
private Sprite dragSprite;
public Tower TowerObject{
	get{
		return towerObject;
	}
}

public int TowerPrice{
	get{
		return towerPrice;
	}
}

public Sprite DragSprite{
	get{
		return dragSprite;
	}
}


}
