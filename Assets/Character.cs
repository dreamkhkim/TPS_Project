using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// 인터페이스 적 
public interface FoundTarget
{
    void EnemyFound(int value);
}

public interface PlayerTarget
{
    void PlayerFound(int value);
}

// 상호작용 문들 
public interface Interaction
{
    void DoorInteraction();
}



public class Character : MonoBehaviour
{
    [SerializeField]
    private int hp;

    [SerializeField]
    private int gunDamage;

    public int GetDamage
    {
        get { return gunDamage; }

        set
        {
            gunDamage = value;
        }
    }

    public virtual int Hp
    {
        get { return hp; }

        set
        {
            hp = value;

            //hp -= gunDamage;

            if(hp <= 0)
            {
                Destroy(gameObject, 3f);
            }
        }
    }


}
