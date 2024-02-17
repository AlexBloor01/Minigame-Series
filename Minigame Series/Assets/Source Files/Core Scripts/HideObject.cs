using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class HideObject : MonoBehaviour
    {
        [Header("Hides Escape for Options")]
        [SerializeField] private float hideTime = 3f;

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(Hide());
        }

        IEnumerator Hide()
        {
            yield return new WaitForSeconds(hideTime);
            gameObject.SetActive(false);
            yield return null;
        }
    }
}


