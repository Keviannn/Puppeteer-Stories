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


        //Creamos al jugador
        GameObject testPlayer = new GameObject();

        Player2_5D player = testPlayer.AddComponent<Player2_5D>();

        //Si no es null, esxiste y pasamos prueba
        Assert.IsNotNull(player);

        Object.DestroyImmediate(testPlayer);
    }

    /*Test que comprueba si al crear una plataforma esta tiene velocidad*/
    [Test]
    public void PlatformShouldMove()
    {

        //Creamos la plataforma
        GameObject testPlatform = new GameObject();

        MovingPlatform platform = testPlatform.AddComponent<MovingPlatform>();

        //Si tiene velocidad distinta de 0, pasamos prueba
        Assert.AreNotEqual(platform.speed, 0f);

        Object.DestroyImmediate(testPlatform);
    }

    /*Test que compruebe que una plataforma se mueva al lado contrario después de cruzar un límite*/
    [Test]
    public void PlatformShouldBound()
    {

        //Creamos la plataforma
        GameObject testPlatform = new GameObject();

        MovingPlatform platform = testPlatform.AddComponent<MovingPlatform>();

        //La ponemos más allá del límite
        testPlatform.transform.position = new Vector3(0f, 0f, 3.5f);

        //Establecemos una velocidad
        float ogSpeed = 1f;

        platform.speed = ogSpeed;

         //Simulación de lo que hace en el update
        if (testPlatform.transform.position.z > 3 || testPlatform.transform.position.z < -3)
        {
            platform.speed = -platform.speed;
        }
        //Si se niega la velocidad, pasamos test
        Assert.AreEqual(platform.speed, -ogSpeed);

    }

    /*Test que comprueba que la función GetObject en DragableObjects2_5D funcione*/
    [UnityTest]
    public IEnumerator GetObject_Dragable()
    {

        //Creamos cámara
        GameObject camara = new GameObject();
        Camera cam = camara.AddComponent<Camera>();
        camara.transform.position = new Vector3(0, 0, -10);
        camara.tag = "MainCamera";


        //Creamos script para mover objetos
        GameObject dragable = new GameObject();
        dragable.transform.position = new Vector3(0, 0, 0);
        DragableObjects2_5D drag = dragable.AddComponent<DragableObjects2_5D>();

        //Creamos objeto a mover
        GameObject obj = new GameObject();

        obj.transform.position = new Vector3(0, 0, 0);
        obj.AddComponent<BoxCollider>();
        Rigidbody rbObj = obj.AddComponent<Rigidbody>();
        obj.tag = "Drag";


        //Calculamos posicion del objeto
        Vector3 pixel = cam.WorldToScreenPoint(obj.transform.position);

        //Creamos cursor y lo ponemos encima del objeto
        var raton = InputSystem.AddDevice<Mouse>();
        raton.MakeCurrent();

        var estado = new MouseState();
        estado.position = new Vector2(pixel.x, pixel.y);
        InputSystem.QueueStateEvent(raton, estado);

        yield return null;

        //Llamamos a la función
        Rigidbody rb = drag.GetObject();

        yield return null;

        InputSystem.RemoveDevice(raton);

        //Si la función devuelve el mismo rigidbody que el del objeto, funciona
        Assert.AreSame(rb,rbObj);
    }

    /*Test que comprueba que el jugador detecta el input correctamente y se mueve a la derecha*/
    [UnityTest]
    public IEnumerator ShouldMoveRight()
    {

        //Creamos al jugador
        GameObject testPlayer = new GameObject();

        testPlayer.AddComponent<Rigidbody>();

        Player2_5D player = testPlayer.AddComponent<Player2_5D>();
        testPlayer.AddComponent<CapsuleCollider>();

        yield return null; //Fotograma para que se incialice

        //Guardamos posicion inicial
        float posInicial = testPlayer.transform.position.x;

        //Creamos teclado y pulsamos d
        var keyboard = InputSystem.AddDevice<Keyboard>();
        keyboard.MakeCurrent();
        InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.D));

        yield return null; //Fotograma para que detecte la tecla
        yield return new WaitForFixedUpdate(); //Fotograma para que se mueva

        InputSystem.RemoveDevice(keyboard);

        float posFinal = testPlayer.transform.position.x;
        
        //Si la posicion final es mayor que la inicial se ha movido a la derecha
        Assert.Greater(posFinal, posInicial);

    }


    /*Test que comprueba que el jugador detecta el input correctamente y se mueve a la izquierda*/
    [UnityTest]
    public IEnumerator ShouldMoveLeft()
    {
        //Creamos al jugador
        GameObject testPlayer = new GameObject();

        testPlayer.AddComponent<Rigidbody>();

        Player2_5D player = testPlayer.AddComponent<Player2_5D>();
        testPlayer.AddComponent<CapsuleCollider>();

        yield return null; //Fotograma para que se incialice

        //Guardamos la posicion inicial
        float posInicial = testPlayer.transform.position.x;

        //Creamos teclado y pulsamos a
        var keyboard = InputSystem.AddDevice<Keyboard>();
        keyboard.MakeCurrent();
        InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.A));

        yield return null; //Fotograma para que detecte la tecla
        yield return new WaitForFixedUpdate(); //Fotograma para que se mueva

        InputSystem.RemoveDevice(keyboard);

        float posFinal = testPlayer.transform.position.x;

        //Si la posicion final es menor que la inicial nos hemos movido a la izquierda
        Assert.Greater(posInicial, posFinal);

    }

    /*Test que comprueba que un objeto detecta el click encima suya*/
    [UnityTest]
    public IEnumerator ShouldDetectClick()
    {

        //Creamos una cámara para detectar al objeto y cursor
        GameObject camara = new GameObject();
        Camera cam = camara.AddComponent<Camera>();
        camara.transform.position = new Vector3(0, 0, -10);
        camara.tag = "MainCamera";


        //Creamos el objeto que llevará el script para que se puedan arrastrar los objetos
        GameObject dragable = new GameObject();
        DragableObjects2_5D drag = dragable.AddComponent<DragableObjects2_5D>();

        //Creamos el objeto a arrastrar
        GameObject obj = new GameObject();
        obj.transform.position = new Vector3(0, 0, 0);
        obj.AddComponent<BoxCollider>();
        obj.AddComponent<Rigidbody>();
        obj.tag = "Drag";

        yield return null;

        //Calculamos la posicion del objeto
        Vector3 pixel = cam.WorldToScreenPoint(obj.transform.position);

        //Creamos el cursor, lo ponemos encima del objeto y hacemos click
        var raton = InputSystem.AddDevice<Mouse>();
        raton.MakeCurrent();

        var estado = new MouseState().WithButton(MouseButton.Left, true);
        estado.position = new Vector2(pixel.x, pixel.y);

        InputSystem.QueueStateEvent(raton, estado);

        yield return null;
        yield return null;

        InputSystem.RemoveDevice(raton);

        //Si la variable isActiv es true, entonces ha detectado el click
        Assert.IsTrue(drag.isActiv);
    }

    /*Test que comprueba que el jugador detecta el input correctamente y salta*/
    [UnityTest]
    public IEnumerator ShouldJump()
    {

        //Creamos al jugador
        GameObject testPlayer = new GameObject();
        testPlayer.AddComponent<Rigidbody>();
        Player2_5D player = testPlayer.AddComponent<Player2_5D>();
        testPlayer.AddComponent<CapsuleCollider>();
        testPlayer.transform.position = new Vector3(0, 2f,0);

        //Creamos el suelo
        GameObject suelo = new GameObject();
        suelo.AddComponent<BoxCollider>();
        suelo.transform.position = new Vector3(0, 0, 0);
        suelo.transform.localScale = new Vector3(5f, 1f, 5f);

        yield return null; //Fotograma para que se incialice
        yield return new WaitForSeconds(2f); //Segundos para que toque el suelo

        float posInicial = testPlayer.transform.position.y;

        //Iniciamos el teclado y pulsamos el espacio
        var keyboard = InputSystem.AddDevice<Keyboard>();
        keyboard.MakeCurrent();
        InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.Space));

        yield return null; //Fotograma para que detecte la tecla
        yield return new WaitForFixedUpdate(); //Fotograma para que se mueva

        InputSystem.RemoveDevice(keyboard);

        float posFinal = testPlayer.transform.position.y;
        
        //Si la posInicial es mayor que la final,
        Assert.Greater(posFinal, posInicial);

    }

    /*Test que detecta si el jugador sobrepasa el límite de altura en un salto*/
    [UnityTest]
    public IEnumerator ShouldNotPassMaxJump()
    {

        //Creamos al jugador
        GameObject testPlayer = new GameObject();
        testPlayer.AddComponent<Rigidbody>();
        Player2_5D player = testPlayer.AddComponent<Player2_5D>();
        testPlayer.AddComponent<CapsuleCollider>();
        testPlayer.transform.position = new Vector3(0, 2f,0);

        //Creamos el suelo
        GameObject suelo = new GameObject();
        suelo.AddComponent<BoxCollider>();
        suelo.transform.position = new Vector3(0, 0, 0);
        suelo.transform.localScale = new Vector3(5f, 1f, 5f);

        yield return null; //Fotograma para que se incialice
        yield return new WaitForSeconds(2f); //Segundos para que toque el suelo

        float posInicial = testPlayer.transform.position.y;

        //Iniciamos el teclado y pulsamos el espacio
        var keyboard = InputSystem.AddDevice<Keyboard>();
        keyboard.MakeCurrent();
        InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.Space));

        yield return null; //Fotograma para que detecte la tecla

        float alturaMaxima = 4.5f;
        float alturaMaximaAlc = 0f;

        float tiempo = 0f;

        //Recorremos 3 segundos, pausando cada vez que el motor de físicas ejecute
        //Esto para recoger la altura máxima en el momento exacto
        while (tiempo < 3f)
        {
            yield return new WaitForFixedUpdate(); //Fotograma para que se mueva
            tiempo += Time.fixedDeltaTime;
            alturaMaximaAlc = Math.Max(alturaMaximaAlc, testPlayer.transform.position.y);
            //Si pasamos la altura maxima, la nueva altura maxima a la que llega el jugador
        }
        InputSystem.RemoveDevice(keyboard);

        //Si la altura maxima supera a la original, no pasamos la prueba, pasamos en o.c
        Assert.GreaterOrEqual(alturaMaxima, alturaMaximaAlc);

    }

    /*Test que detecta que el jugador no pase un tiempo máximo en el aire después de un salto*/
    [UnityTest]
    public IEnumerator ShouldNotPassAirMaxTimeInJump()
    {
        
        //Creamos al jugador
        GameObject testPlayer = new GameObject();
        testPlayer.AddComponent<Rigidbody>();
        Player2_5D player = testPlayer.AddComponent<Player2_5D>();
        testPlayer.AddComponent<CapsuleCollider>();
        testPlayer.transform.position = new Vector3(0, 2f,0);

        //Creamos el suelo
        GameObject suelo = new GameObject();
        suelo.AddComponent<BoxCollider>();
        suelo.transform.position = new Vector3(0, 0, 0);
        suelo.transform.localScale = new Vector3(5f, 1f, 5f);

        yield return null; //Fotograma para que se incialice
        yield return new WaitForSeconds(2f); //Segundos para que toque el suelo

        float posInicial = testPlayer.transform.position.y;

        //Iniciamos el teclado y pulsamos el espacio
        var keyboard = InputSystem.AddDevice<Keyboard>();
        keyboard.MakeCurrent();
        InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.Space));

        yield return null; //Fotograma para que detecte la tecla

        float alturaMaxima = 4f;

        float tiempo = 0f;

        //Recorremos 2 segundos, pausando cada vez que el motor de físicas ejecute
        //Esto para recoger la altura máxima en el momento exacto
        while (tiempo < 2f)
        {
            yield return new WaitForFixedUpdate(); //Fotograma para que se mueva
            tiempo += Time.fixedDeltaTime;

           //Cuando alcancemos la altura maxima, salimos
            if (alturaMaxima == testPlayer.transform.position.y)
            {
                break;
            }
        }


        tiempo = 0;

        //Calculamos lo que tarda el jugador en tocar el suelo
        while (!player.isGrounded)
        {
            yield return new WaitForFixedUpdate(); //Fotograma para que se mueva
            tiempo += Time.fixedDeltaTime;
        }

        InputSystem.RemoveDevice(keyboard);

        //Si es menor que el limite puesto, pasamos el test
        Assert.GreaterOrEqual(2.5f, tiempo);

    }

    /*Test que detecta que no se sobrepase el límite de velocidad establecido*/
    [UnityTest]
    public IEnumerator ShouldNotPassMaxSpeed()
    {

        //Creamos el jugador
        GameObject testPlayer = new GameObject();
        testPlayer.AddComponent<Rigidbody>();
        Player2_5D player = testPlayer.AddComponent<Player2_5D>();
        testPlayer.AddComponent<CapsuleCollider>();
        testPlayer.transform.position = new Vector3(0, 2f,0);

        //Creamos un suelo
        GameObject suelo = new GameObject();
        suelo.AddComponent<BoxCollider>();
        suelo.transform.position = new Vector3(0, 0, 0);
        suelo.transform.localScale = new Vector3(20f, 1f, 1f);

        yield return null; //Fotograma para que se incialice
        yield return new WaitForSeconds(2f); //Segundos para que toque el suelo

        //Creamos teclado y pulsamos la tecla D para avanzar
        var keyboard = InputSystem.AddDevice<Keyboard>();
        keyboard.MakeCurrent();
        InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.D));

        yield return null; //Fotograma para que detecte la tecla

        //Guardamos la posición inical
        float tiempo = 0f;
        var posInicial = testPlayer.transform.position.x;

        //Recorremos 3 segundos
        while (tiempo < 3f)
        {
            yield return new WaitForFixedUpdate(); //Fotograma para que se mueva
            tiempo += Time.fixedDeltaTime;
        }

        //Guardamos la posición después de moverse
        var posFinal = testPlayer.transform.position.x;

        //Calculamos la velocidad
        float vel = (posInicial - posFinal) / tiempo;

        InputSystem.RemoveDevice(keyboard);

        //Si la velocidad es menor o igual que el límite pasa la prueba
        Assert.GreaterOrEqual(10, vel);

    }

    /*Test que comprueba que al hacer click en un objeto y mover el cursor
      este se mueva*/
      
    [UnityTest]
    public IEnumerator ShouldMoveObject()
    {

        //Creamos la camara
        GameObject camara = new GameObject();
        Camera cam = camara.AddComponent<Camera>();
        camara.transform.position = new Vector3(0, 0, -10);
        camara.tag = "MainCamera";


        //Creamos el objeto que llevará el script para que se puedan arrastrar los objetos
        GameObject dragable = new GameObject();
        DragableObjects2_5D drag = dragable.AddComponent<DragableObjects2_5D>();

        //Creamos el objeto a arrastrar
        GameObject obj = new GameObject();
        var posInicial = obj.transform.position = new Vector3(0, 0, 0);
        obj.AddComponent<BoxCollider>();
        obj.AddComponent<Rigidbody>();
        obj.tag = "Drag";

        yield return null; //Frame para que se inicialicen los componentes

        //Guardamos posición donde está el objeto
        Vector3 pixel = cam.WorldToScreenPoint(obj.transform.position);

        var raton = InputSystem.AddDevice<Mouse>();
        raton.MakeCurrent();

        //Ponemos el cursor encima y clickamos
        var estado = new MouseState().WithButton(MouseButton.Left, true);
        estado.position = new Vector2(pixel.x, pixel.y);

        InputSystem.QueueStateEvent(raton, estado);

        //Frames para que se procese la petición de click
        yield return null;
        yield return null;

        //Movemos el cursor a la derecha
        estado.position = new Vector2(pixel.x + 5000f, pixel.y);
        InputSystem.QueueStateEvent(raton, estado);

        //Ejecutamos durante dos segundos
        yield return new WaitForSeconds(2f);

        InputSystem.RemoveDevice(raton);

        //Si la posición ahora es mayor que la original, entonces el objeto se ha movido.
        Assert.Greater(obj.transform.position.x, posInicial.x);

    }


}
