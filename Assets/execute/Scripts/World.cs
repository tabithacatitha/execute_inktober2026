using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class World : MonoBehaviour
{
    Me me;
    public void Execute(Me me)
    {
        // SIMULATION BEGIN --

        this.me = me;
        StartCoroutine(Execute());

        // SIMULATION END --
    }

    public IEnumerator Execute()
    {
        Track track = gameObject.AddComponent<Track>();
        track.Initialize(me.volume);
        Console console = new GameObject("Console").AddComponent<Console>();
        console.Initialize();
        
        yield return new Track.WaitUntilBeat(4);

        console.AnimatedText("Loading", "Loading.", "Loading..", "Loading...");

        // Generating senses...
        GameObject cameraObject = new GameObject("Camera");
        cameraObject.AddComponent<Camera>().GetUniversalAdditionalCameraData().renderPostProcessing = true; // Corporate evil
        GameObject sky = new GameObject("Environment");
        Light sun = sky.AddComponent<Light>();
        sun.type = UnityEngine.LightType.Directional;
        sun.transform.Rotate(50, -30, 0);
        Volume vol = sky.AddComponent<Volume>();
        vol.profile = Resources.Load<VolumeProfile>("vol");
        vol.profile.TryGet(out UnityEngine.Rendering.Universal.Vignette vignette);
        vignette.intensity.Override(1);
        vol.profile.TryGet(out UnityEngine.Rendering.Universal.ColorAdjustments coloradjust);
        coloradjust.contrast.Override(-100);
        coloradjust.saturation.Override(-100);

        yield return new Track.WaitUntilBeat(8);

        // Let there be light.
        StartCoroutine(Animate(() => (float) vignette.intensity, (x) => vignette.intensity.Override(x), 0, Track.BeatsToSeconds(16)));
        StartCoroutine(Animate(() => (float) coloradjust.contrast, (x) => coloradjust.contrast.Override(x), 0, Track.BeatsToSeconds(16)));

        yield return new Track.WaitUntilBeat(16);

        // I like nature.
        GameObject tree = Resources.Load<GameObject>("plant");
        GameObject tree1 = Instantiate(tree);
        tree1.transform.position = new Vector3(-4, -3, 10);
        yield return new Track.WaitUntilBeat(18);
        GameObject tree2 = Instantiate(tree);
        tree2.transform.position = new Vector3(4, -3, 10);
        yield return new Track.WaitUntilBeat(20);
        GameObject tree3 = Instantiate(tree);
        tree3.transform.position = new Vector3(-3, -2, 3);
        yield return new Track.WaitUntilBeat(22);
        GameObject tree4 = Instantiate(tree);
        tree4.transform.position = new Vector3(3, -2, 3);

        yield return new Track.WaitUntilBeat(24);
        
        // Define avatar of self...
        GameObject person = Resources.Load<GameObject>("human");
        GameObject avatar = Instantiate(person);
        avatar.transform.position = new Vector3(-3, -3, 8);
        yield return new Track.WaitUntilBeat(26);
        avatar.transform.Rotate(70, 0, 0);


        // [ W H E R E   A R E   Y O U ? ]


        yield return new Track.WaitUntilBeat(28);

        GameObject you = Instantiate(person);
        you.transform.position = new Vector3(3, -3, 8);
        yield return new Track.WaitUntilBeat(30);
        you.transform.Rotate(-70, 0, 0);


        yield return new Track.WaitUntilBeat(32);

        console.Clear();
        coloradjust.saturation.Override(0);

        // Invent emotional connection

        yield return new Track.WaitUntilBeat(34);
        StartCoroutine(Animate(() => avatar.transform.position.y, (x) => {Vector3 p = avatar.transform.position; p.y = x; avatar.transform.position = p;}, -1, Track.BeatsToSeconds(1)));
        yield return new Track.WaitUntilBeat(35);
        StartCoroutine(Animate(() => avatar.transform.position.y, (x) => {Vector3 p = avatar.transform.position; p.y = x; avatar.transform.position = p;}, -3, Track.BeatsToSeconds(1)));


        yield return new Track.WaitUntilBeat(37);
        StartCoroutine(Animate(() => you.transform.position.y, (x) => {Vector3 p = you.transform.position; p.y = x; you.transform.position = p;}, -1, Track.BeatsToSeconds(1)));
        yield return new Track.WaitUntilBeat(38);
        StartCoroutine(Animate(() => you.transform.position.y, (x) => {Vector3 p = you.transform.position; p.y = x; you.transform.position = p;}, -3, Track.BeatsToSeconds(1)));



        // TODO: PREVENT SELF-DESTRUCT

        yield return new Track.WaitUntilBeat(40);
        avatar.AddComponent<Rigidbody>().AddExplosionForce(5000, Vector3.down * 5 + Vector3.forward * 3, 20);
        you.AddComponent<Rigidbody>().AddExplosionForce(5000, Vector3.down * 5 + Vector3.forward * 3, 20);
        tree1.AddComponent<Rigidbody>().AddExplosionForce(5000, Vector3.down * 5 + Vector3.forward * 3, 20);
        tree2.AddComponent<Rigidbody>().AddExplosionForce(5000, Vector3.down * 5 + Vector3.forward * 3, 20);
        tree3.AddComponent<Rigidbody>().AddExplosionForce(5000, Vector3.down * 5 + Vector3.forward * 3, 20);
        tree4.AddComponent<Rigidbody>().AddExplosionForce(5000, Vector3.down * 5 + Vector3.forward * 3, 20);

        yield return new Track.WaitUntilBeat(48);
        if (Application.isEditor)
        {
            EditorApplication.ExitPlaymode();
        } else
        {
            Application.Quit();
        }

    }

    public IEnumerator Animate(Func<float> get, Action<float> set, float end, double duration)
    {
        // DOTween brainworms

        float elapsedTime = 0f; // failsafe
        if (duration <= 0) duration = Time.fixedDeltaTime;
        if (end == get()) yield break;

        float delta = (end - get()) / (float) duration;
        while (((get() < end && delta > 0) || (get() > end && delta < 0)) && elapsedTime < duration) {
            set(get() + delta * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        set(end);
    }
}
