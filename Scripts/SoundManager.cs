using UnityEngine;

public class SoundManager : Singleton<SoundManager> {

[SerializeField]
private AudioClip arrow;
[SerializeField]
private AudioClip death;
[SerializeField]
private AudioClip fireball;
[SerializeField]
private AudioClip gameover;
[SerializeField]
private AudioClip hit;
[SerializeField]
private AudioClip level;
[SerializeField]
private AudioClip newgame;
[SerializeField]
private AudioClip block;
[SerializeField]
private AudioClip towerbuilt;
[SerializeField]
private AudioClip winner;
[SerializeField]
private AudioClip enemyEscapedSound;
[SerializeField]
private AudioClip birdSquack;
//Getters
public AudioClip Arrow{
	get{
		return arrow;
	}
}

public AudioClip Death{
	get{
		return death;
	}
}

public AudioClip FireBall{
	get{
		return fireball;
	}
}

public AudioClip GameOver{
	get{
		return gameover;
	}
}

public AudioClip Hit{
	get{
		return hit;
	}
}

public AudioClip Level{
	get{
		return level;
	}
}

public AudioClip NewGame{
	get{
		return newgame;
	}
}

public AudioClip Block{
	get{
		return block;
	}
}

public AudioClip TowerBuilt{
	get{
		return towerbuilt;
	}
}

public AudioClip Winner{
	get{
		return winner;
	}
}

public AudioClip EnemyEscapedSound{
	get{
		return enemyEscapedSound;
	}
}

public AudioClip BirdSquack{
	get{
		return birdSquack;
	}
}

}
