using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public string title;          // ��������� �������
    public string description;    // ��������� �������� �������
    public bool isCompleted;      // ������ ���������� �������

    // �����������, ������� ��������� 3 ���������
    public Quest(string title, string description, bool isCompleted)
    {
        this.title = title;
        this.description = description;
        this.isCompleted = isCompleted;
    }
}
