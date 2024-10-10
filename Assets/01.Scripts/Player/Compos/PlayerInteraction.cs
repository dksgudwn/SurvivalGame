using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour, IPlayerComponent
{
    [SerializeField] private Camera PlayerCam;
    [SerializeField][Range(1f, 15f)] private float InteractionRange = 10f;
    [SerializeField] private LayerMask InteractionLayer;

    private Player _player;
    private IInteractable interactionObject;

    public void Initialize(Player player)
    {
        _player = player;
    }

    public void AfterInitialize()
    {
    }

    private void Update()
    {
        FindInteractionRay();
        ActionInteraction();
    }

    private void FindInteractionRay() //상호작용 오브젝트 감지 및 데이터 가져오기
    {
        Ray findRay = new Ray(PlayerCam.transform.position, PlayerCam.transform.forward);
        RaycastHit hitData;

        if (Physics.Raycast(findRay, out hitData, InteractionRange, InteractionLayer))
        {
            hitData.collider.TryGetComponent(out interactionObject);
        }
        else
        {
            interactionObject = null;
        }
    }

    private void ActionInteraction() // 상호작용 오브젝트의 상호작용 액션 실행
    {
        if(interactionObject == null) return;
        if(Input.GetMouseButtonDown(1)) interactionObject.DoInteractionEvent();
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(PlayerCam.transform.position, PlayerCam.transform.forward, Color.red);
    }
}
