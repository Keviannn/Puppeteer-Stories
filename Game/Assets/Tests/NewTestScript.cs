using System.Collections;
using NUnit.Framework;
using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
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


    //Falla ya que en player2_5D no hay velocidad asignada... Si se le asigna una en el script funciona

    [UnityTest]
    public IEnumerator ShouldMoveRight()
    {

        GameObject testPlayer = new GameObject();

        testPlayer.AddComponent<Rigidbody>();

        Player2_5D player = testPlayer.AddComponent<Player2_5D>();

        yield return null; //Fotograma para que se incialice

        float posInicial = testPlayer.transform.position.x;

        var keyboard = InputSystem.AddDevice<Keyboard>();
        keyboard.MakeCurrent();
        InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.D));
        InputSystem.Update();

        yield return null; //Fotograma para que detecte la tecla
        yield return new WaitForFixedUpdate(); //Fotograma para que se mueva

        InputSystem.RemoveDevice(keyboard);

        float posFinal = testPlayer.transform.position.x;

        Assert.Greater(posFinal,posInicial);
        
    }

    
}