using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class NPCResponse
{
    [TextArea(3, 5)]
    public List<string> responses; // ������ ������ NPC
}

public class NPCDialogue : MonoBehaviour
{
    [TextArea(3, 5)]
    public string npcLine;

    [TextArea(2, 2)]
    public List<string> playerChoices = new List<string>();

    public List<NPCResponse> npcResponses = new List<NPCResponse>();  // ���������� ������������� �����

    public float interactionDistance = 3f;

    private bool isDialogueActive = false;  // ���� ��� ��������, ������� �� ������

    private void Update()
    {
        // ��������� ��������� ������� "E", ���� ������ ��� �������
        if (Input.GetKeyDown(KeyCode.E) && !isDialogueActive && IsPlayerLookingAtMe())
        {
            Dictionary<int, List<string>> responseDict = new Dictionary<int, List<string>>();

            for (int i = 0; i < playerChoices.Count; i++)
            {
                if (i < npcResponses.Count)
                {
                    responseDict.Add(i + 1, npcResponses[i].responses);  // ��������� ������ ������ �� ������ NPCResponse
                }
            }

            DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
            if (dialogueManager != null)
            {
                isDialogueActive = true;  // ������ �����������
                dialogueManager.StartDialogue(npcLine, playerChoices, responseDict);
            }
        }
    }

    private bool IsPlayerLookingAtMe()
    {
        Camera mainCamera = Camera.main;
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        RaycastHit hit;
        return Physics.Raycast(ray, out hit, interactionDistance) && hit.collider.gameObject == gameObject;
    }

    // ��� ���������� �������
    public void EndDialogue()
    {
        isDialogueActive = false;
    }
}
