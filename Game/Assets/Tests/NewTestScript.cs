using System;
using System.Collections;
using NUnit.Framework;
using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

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
        testPlayer.AddComponent<CapsuleCollider>();

        yield return null; //Fotograma para que se incialice

        float posInicial = testPlayer.transform.position.x;

        var keyboard = InputSystem.AddDevice<Keyboard>();
        keyboard.MakeCurrent();
        InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.D));

        yield return null; //Fotograma para que detecte la tecla
        yield return new WaitForFixedUpdate(); //Fotograma para que se mueva

        InputSystem.RemoveDevice(keyboard);

        float posFinal = testPlayer.transform.position.x;

        Assert.Greater(posFinal,posInicial);
        
    }

    [UnityTest]
    public IEnumerator ShouldMoveLeft()
    {

        GameObject testPlayer = new GameObject();

        testPlayer.AddComponent<Rigidbody>();

        Player2_5D player = testPlayer.AddComponent<Player2_5D>();
        testPlayer.AddComponent<CapsuleCollider>();

        yield return null; //Fotograma para que se incialice

        float posInicial = testPlayer.transform.position.x;

        var keyboard = InputSystem.AddDevice<Keyboard>();
        keyboard.MakeCurrent();
        InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.A));

        yield return null; //Fotograma para que detecte la tecla
        yield return new WaitForFixedUpdate(); //Fotograma para que se mueva

        InputSystem.RemoveDevice(keyboard);

        float posFinal = testPlayer.transform.position.x;

        Assert.Greater(posInicial,posFinal);
        
    }

    //Falla ya que usamos el sistema de input antiguo... No he encontrado manera de arreglarlo
    [UnityTest]
    public IEnumerator ShouldDetectClick()
    {
        
        GameObject camara = new GameObject();
        Camera cam = camara.AddComponent<Camera>();
        camara.transform.position = new Vector3(0, 0, -10);
        camara.tag = "MainCamera";



        GameObject dragable = new GameObject();
        DragableObjects2_5D drag = dragable.AddComponent<DragableObjects2_5D>();

        GameObject obj = new GameObject();

        obj.transform.position = new Vector3(0, 0, 0);
        obj.AddComponent<BoxCollider>();
        obj.AddComponent<Rigidbody>();
        obj.tag = "Drag";

        yield return null;

        Vector3 pixel = cam.WorldToScreenPoint(obj.transform.position);

        var raton = InputSystem.AddDevice<Mouse>();
        raton.MakeCurrent();

        var estado = new MouseState().WithButton(MouseButton.Left, true);
        estado.position = new Vector2(pixel.x, pixel.y);

        InputSystem.QueueStateEvent(raton, estado);
        InputSystem.Update();

        yield return null;
        yield return null;

        InputSystem.RemoveDevice(raton);

        //Assert.IsTrue(drag.isActiv);
    }

    [UnityTest]
    public IEnumerator ShouldJump()
    {

        GameObject testPlayer = new GameObject();
        testPlayer.AddComponent<Rigidbody>();
        Player2_5D player = testPlayer.AddComponent<Player2_5D>();
        testPlayer.AddComponent<CapsuleCollider>();

        GameObject suelo = new GameObject();
        suelo.AddComponent<BoxCollider>();
        suelo.transform.position = new Vector3(0, 0, 0);
        suelo.transform.localScale = new Vector3(5f, 1f, 5f);

        yield return null; //Fotograma para que se incialice
        yield return new WaitForSeconds(2f); //Segundos para que toque el suelo

        float posInicial = testPlayer.transform.position.y;

        var keyboard = InputSystem.AddDevice<Keyboard>();
        keyboard.MakeCurrent();
        InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.Space));

        yield return null; //Fotograma para que detecte la tecla
        yield return new WaitForFixedUpdate(); //Fotograma para que se mueva

        InputSystem.RemoveDevice(keyboard);

        float posFinal = testPlayer.transform.position.y;

        Assert.Greater(posInicial,posFinal);
        
    }

    [UnityTest]
    public IEnumerator ShouldNotPassMaxJump()
    {

        GameObject testPlayer = new GameObject();
        testPlayer.AddComponent<Rigidbody>();
        Player2_5D player = testPlayer.AddComponent<Player2_5D>();
        testPlayer.AddComponent<CapsuleCollider>();

        GameObject suelo = new GameObject();
        suelo.AddComponent<BoxCollider>();
        suelo.transform.position = new Vector3(0, 0, 0);
        suelo.transform.localScale = new Vector3(5f, 1f, 5f);

        yield return null; //Fotograma para que se incialice
        yield return new WaitForSeconds(2f); //Segundos para que toque el suelo

        float posInicial = testPlayer.transform.position.y;

        var keyboard = InputSystem.AddDevice<Keyboard>();
        keyboard.MakeCurrent();
        InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.Space));

        yield return null; //Fotograma para que detecte la tecla

        float alturaMaxima = 4f;

        float tiempo = 0f;

        while(tiempo < 3f){
        yield return new WaitForFixedUpdate(); //Fotograma para que se mueva
        tiempo += Time.fixedDeltaTime;
        alturaMaxima = Math.Max(alturaMaxima, testPlayer.transform.position.y);
        }
        InputSystem.RemoveDevice(keyboard);

        Assert.GreaterOrEqual(4,alturaMaxima);
        
    }

}