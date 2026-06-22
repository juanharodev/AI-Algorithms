using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void PlayerHealthTest()
    {
        //Arrange 
        GameObject playerGameObject = new GameObject();
        Health playerHealth = playerGameObject.AddComponent<Health>();

        int damage = 30;
        //Act 
        int result = playerHealth.TakeDamage(damage);

        //Assert
        Assert.AreEqual(20, result);
    }

    // A Test behaves as an ordinary method
    [Test]
    public void PlayerHealthZeroHealth()
    {
        //Arrange 
        GameObject playerGameObject = new GameObject();
        Health playerHealth = playerGameObject.AddComponent<Health>();

        int damage = 9000000;

        //Act 
        int result = playerHealth.TakeDamage(damage);

        //Assert
        Assert.AreEqual(0, result);
    }

    // A Test behaves as an ordinary method
    [Test]
    public void PlayerHealthHeal()
    {
        //Arrange 
        GameObject playerGameObject = new GameObject();
        Health playerHealth = playerGameObject.AddComponent<Health>();

        int heal = 30;

        //Act 
        int result = playerHealth.Heal(heal);

        //Assert
        Assert.AreEqual(80, result);
    }


    [Test]
    public void PlayerHealthMaxHeal()
    {
        //Arrange 
        GameObject playerGameObject = new GameObject();
        Health playerHealth = playerGameObject.AddComponent<Health>();

        int heal = 999999999;

        //Act 
        int result = playerHealth.Heal(heal);

        //Assert
        Assert.AreEqual(100, result);
    }
}
