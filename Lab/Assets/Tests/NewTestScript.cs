using System.Collections;
using NUnit.Framework;
using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.TestTools;

public class NewTestScript
{
    // A Test behaves as an ordinary method
    [Test]
    public void NewTestScriptSimplePasses()
    {
        // Use the Assert class to test conditions
    }

    [Test]
    public void ShouldCreatePlayer()
    {
        GameObject testPlayer = new GameObject();

        Player2_5D player = testPlayer.AddComponent<Player2_5D>();

        Assert.IsNotNull(player);

        Object.DestroyImmediate(testPlayer);
    }

    [Test]
    public void PlatformShouldMove()
    {
        GameObject testPlatform = new GameObject();

        MovingPlatform platform = testPlatform.AddComponent<MovingPlatform>();

        Assert.Greater(platform.speed, 0f);

        Object.DestroyImmediate(testPlatform);
    }

    [Test]
    public void PlatformShouldBound()
    {
        GameObject testPlatform = new GameObject();

        MovingPlatform platform = testPlatform.AddComponent<MovingPlatform>();

        testPlatform.transform.position = new Vector3(0f, 0f, 3.5f);

        float ogSpeed = 1f;

        platform.speed = ogSpeed;

        if(testPlatform.transform.position.z > 3 || testPlatform.transform.position.z < -3){
	    platform.speed = -platform.speed;
	    }

        Assert.AreEqual(platform.speed, -ogSpeed);

    }
}