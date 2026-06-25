using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
public class OneWayPlatform : MonoBehaviour
{
    private PlatformEffector2D effector;
    void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
    }

    void Update()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        if (kb.sKey.wasPressedThisFrame || kb.downArrowKey.wasPressedThisFrame)
        {
            StartCoroutine(DisableCollisionRoutine());
        }
    }
    private IEnumerator DisableCollisionRoutine()
    {
        effector.rotationalOffset = 180f;
        yield return new WaitForSeconds(0.5f);
        effector.rotationalOffset = 0f;
    }
}