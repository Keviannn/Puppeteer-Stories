using System;
using System.Collections;
using NUnit.Framework;
using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
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

    /*Test que comprueba si el jugador se crea correctamente*/
    [Test]
    public void ShouldCreatePlayer()
    {
        GameObject testPlayer = new GameObject();

        Player2_5D player = testPlayer.AddComponent<Player2_5D>();

        Assert.IsNotNull(player);

        Object.DestroyImmediate(testPlayer);
    }

    /*Test que comprueba si al crear una plataforma esta tiene velocidad*/
    [Test]
    public void PlatformShouldMove()
    {
        GameObject testPlatform = new GameObject();

        MovingPlatform platform = testPlatform.AddComponent<MovingPlatform>();

        Assert.AreNotEqual(platform.speed, 0f);

        Object.DestroyImmediate(testPlatform);
    }

    /*Test que compruebe que una plataforma se mueva al lado contrario después de cruzar un límite*/
    [Test]
    public void PlatformShouldBound()
    {
        GameObject testPlatform = new GameObject();

        MovingPlatform platform = testPlatform.AddComponent<MovingPlatform>();

        testPlatform.transform.position = new Vector3(0f, 0f, 3.5f);

        float ogSpeed = 1f;

        platform.speed = ogSpeed;

        if (testPlatform.transform.position.z > 3 || testPlatform.transform.position.z < -3)
        {
            platform.speed = -platform.speed;
        }

        Assert.AreEqual(platform.speed, -ogSpeed);

    }

    /*Test que comprueba que la función GetObject en DragableObjects2_5D funcione*/
    [UnityTest]
    public IEnumerator GetObject_Dragable()
    {

        GameObject camara = new GameObject();
        Camera cam = camara.AddComponent<Camera>();
        camara.transform.position = new Vector3(0, 0, -10);
        camara.tag = "MainCamera";



        GameObject dragable = new GameObject();
        dragable.transform.position = new Vector3(0, 0, 0);
        DragableObjects2_5D drag = dragable.AddComponent<DragableObjects2_5D>();

        GameObject obj = new GameObject();

        obj.transform.position = new Vector3(0, 0, 0);
        obj.AddComponent<BoxCollider>();
        Rigidbody rbObj = obj.AddComponent<Rigidbody>();
        obj.tag = "Drag";


        Vector3 pixel = cam.WorldToScreenPoint(obj.transform.position);

        var raton = InputSystem.AddDevice<Mouse>();
        raton.MakeCurrent();

        var estado = new MouseState();
        estado.position = new Vector2(pixel.x, pixel.y);
        InputSystem.QueueStateEvent(raton, estado);

        yield return null;

        Rigidbody rb = drag.GetObject();

        yield return null;

        InputSystem.RemoveDevice(raton);

        Assert.AreSame(rb,rbObj);
    }

    /*Test que comprueba que el jugador detecta el input correctamente y se mueve a la derecha*/
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

        Assert.Greater(posFinal, posInicial);

    }


    /*Test que comprueba que el jugador detecta el input correctamente y se mueve a la izquierda*/
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

        Assert.Greater(posInicial, posFinal);

    }

    /*Test que comprueba que un objeto detecta el click encima suya*/
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

        yield return null;
        yield return null;

        InputSystem.RemoveDevice(raton);

        Assert.IsTrue(drag.isActiv);
    }

    /*Test que comprueba que el jugador detecta el input correctamente y salta*/
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

        Assert.Greater(posInicial, posFinal);

    }

    /*Test que detecta si el jugador sobrepasa el límite de altura en un salto*/
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

        while (tiempo < 3f)
        {
            yield return new WaitForFixedUpdate(); //Fotograma para que se mueva
            tiempo += Time.fixedDeltaTime;
            alturaMaxima = Math.Max(alturaMaxima, testPlayer.transform.position.y);
        }
        InputSystem.RemoveDevice(keyboard);

        Assert.GreaterOrEqual(4, alturaMaxima);

    }

    /*Test que detecta que el jugador no pase un tiempo máximo en el aire después de un salto*/
    [UnityTest]
    public IEnumerator ShouldNotPassAirMaxTimeInJump()
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

        while (tiempo < 2f)
        {
            yield return new WaitForFixedUpdate(); //Fotograma para que se mueva
            tiempo += Time.fixedDeltaTime;

            if (alturaMaxima == testPlayer.transform.position.y)
            {
                break;
            }
        }

        tiempo = 0;
        while (testPlayer.transform.position.y > 0)
        {
            yield return new WaitForFixedUpdate(); //Fotograma para que se mueva
            tiempo += Time.fixedDeltaTime;
        }

        InputSystem.RemoveDevice(keyboard);

        Assert.GreaterOrEqual(1.5f, tiempo);

    }

    /*Test que detecta que no se sobrepase el límite de velocidad establecido*/
    [UnityTest]
    public IEnumerator ShouldNotPassMaxSpeed()
    {

        GameObject testPlayer = new GameObject();
        testPlayer.AddComponent<Rigidbody>();
        Player2_5D player = testPlayer.AddComponent<Player2_5D>();
        testPlayer.AddComponent<CapsuleCollider>();

        GameObject suelo = new GameObject();
        suelo.AddComponent<BoxCollider>();
        suelo.transform.position = new Vector3(0, 0, 0);
        suelo.transform.localScale = new Vector3(20f, 1f, 1f);

        yield return null; //Fotograma para que se incialice
        yield return new WaitForSeconds(2f); //Segundos para que toque el suelo


        var keyboard = InputSystem.AddDevice<Keyboard>();
        keyboard.MakeCurrent();
        InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.D));

        yield return null; //Fotograma para que detecte la tecla

        float tiempo = 0f;
        var posInicial = testPlayer.transform.position.x;

        while (tiempo < 3f)
        {
            yield return new WaitForFixedUpdate(); //Fotograma para que se mueva
            tiempo += Time.fixedDeltaTime;
        }

        var posFinal = testPlayer.transform.position.x;

        float vel = (posInicial - posFinal) / tiempo;

        InputSystem.RemoveDevice(keyboard);

        Assert.GreaterOrEqual(10, vel);

    }

    /*Test que comprueba que al hacer click en un objeto y mover el cursor
      este se mueva*/
      
    [UnityTest]
    public IEnumerator ShouldMoveObject()
    {

        GameObject camara = new GameObject();
        Camera cam = camara.AddComponent<Camera>();
        camara.transform.position = new Vector3(0, 0, -10);
        camara.tag = "MainCamera";



        GameObject dragable = new GameObject();
        DragableObjects2_5D drag = dragable.AddComponent<DragableObjects2_5D>();

        GameObject obj = new GameObject();

        var posInicial = obj.transform.position = new Vector3(0, 0, 0);
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

        yield return null;
        yield return null;

        estado.position = new Vector2(pixel.x + 5000f, pixel.y);
        InputSystem.QueueStateEvent(raton, estado);

        yield return new WaitForSeconds(2f);

        InputSystem.RemoveDevice(raton);

        Assert.Greater(obj.transform.position.x, posInicial.x);

    }


}
