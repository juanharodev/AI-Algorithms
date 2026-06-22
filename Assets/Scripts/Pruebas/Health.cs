using UnityEngine;

public class Health : MonoBehaviour
{
    int currentHealt = 50;
    int maxHealth = 100;

    public int TakeDamage(int _damage)
    {
        currentHealt -= Mathf.Abs(_damage);
        currentHealt = Mathf.Max(0, currentHealt);
        return currentHealt;
    }

    public int Heal(int _amount)
    {
        currentHealt += Mathf.Abs(_amount);
        currentHealt = Mathf.Min(maxHealth, currentHealt);
        return currentHealt;
    }
}
