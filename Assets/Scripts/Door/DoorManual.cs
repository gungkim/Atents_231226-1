using System.Collections;
using TMPro;
using UnityEngine;

public class DoorManual : DoorBase, IInteracable
{
    TextMeshPro text;

    bool isOpen = false;

    public float coolTime = 0.5f;

    float currentCoolTime = 0;

    public bool CanUse => currentCoolTime < 0.0f;

    protected override void Awake()
    {
        base.Awake();
        text = GetComponentInChildren<TextMeshPro>(true);
    }

    void Update()
    {
        currentCoolTime -= Time.deltaTime;
    }

    public void Use()
    {
        if (CanUse)
        {
            if (isOpen)
            {
                Close();
                isOpen = false;
            }
            else
            {
                Open();
                isOpen = true;
            }
            currentCoolTime = coolTime;
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 cameraToDoor = transform.position - Camera.main.transform.position;

            float angle = Vector3.Angle(transform.forward, cameraToDoor);

            if (angle > 90.0f)
            {
                text.transform.rotation = transform.rotation * Quaternion.Euler(0, 180, 0);
            }
            else
            {
                text.transform.rotation = transform.rotation;
            }

            text.gameObject.SetActive(true);
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            text.gameObject.SetActive(false);
        }
    }
}
