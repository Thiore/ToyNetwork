using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gomoku_Logic : MonoBehaviour
{
    // �̹� �а͵� 
    private List<Chip> Black_Chip = new List<Chip>();
    private List<Chip> White_Chip = new List<Chip>();
    [SerializeField] public GameObject result_Panel; //�̰� ���� 
    [SerializeField] private Text winner;

    // ���� ���� ����� ����
    private void Check_Chip(Player player, Chip lastChip)
    {
        List<Chip> playerChips = (1 == player.connectcount % 2) ? Black_Chip : White_Chip;

        // �ٵϾ��� 5���� �ȵǴ� ���
        if (playerChips.Count < 5)
        {
            return;
        }

        // �� ���� ������ üũ
        if (Check_Direction(playerChips, lastChip, 1, 0) ||  // ����
            Check_Direction(playerChips, lastChip, 0, 1) ||  // ����
            Check_Direction(playerChips, lastChip, 1, 1) ||  // ������ �밢��
            Check_Direction(playerChips, lastChip, 1, -1))   // ����� �밢��
        {
            EndGame(player, playerChips.ToArray());
        }
    }

    // Ư�� �������� ���ӵ� ���� ���� Ȯ��
    private bool Check_Direction(List<Chip> chips, Chip lastChip, int dx, int dy)
    {
        int count = 1;

        // ������ üũ
        count += CountChipsInDirection(chips, lastChip, dx, dy);
        // ������ üũ
        count += CountChipsInDirection(chips, lastChip, -dx, -dy);

        return count >= 5;
    }

    // �־��� �������� ���ӵ� ���� �� ��ȯ
    private int CountChipsInDirection(List<Chip> chips, Chip lastChip, int dx, int dy)
    {
        int count = 0;
        int nextRow = lastChip.Row + dx;
        int nextCol = lastChip.Col + dy;

        while (chips.Exists(chip => chip.Row == nextRow && chip.Col == nextCol))
        {
            count++;
            nextRow += dx;
            nextCol += dy;
        }

        return count;
    }

    // ���� ���� (�߰� ���� �ʿ�)
    private void EndGame(Player player, Chip[] chips)
    {
        string winner = (1 == player.connectcount % 2) ? "���" : "���";
        Debug.Log($"{winner} �¸�");
        result_Panel.SetActive(true);
        this.winner.text = ($"{winner} �¸�!");
        // ���� ���� ���� ����

    }

    //==============================================================================================================================================
    #region ������ Ȯ�� �ʿ� �޼���

    // ���� ��, ȣ���ؾ� �ϴ� �޼���
    public void AddChip(Chip chip, Player player)
    {
        if (1 == player.connectcount % 2)
        {
            Black_Chip.Add(chip);
        }
        else
        {
            White_Chip.Add(chip);
        }

        // ���� �߰��� �� ��� ���� ���θ� üũ�մϴ�.
        Check_Chip(player, chip);
    }

    //true�� ��ȯ�Ǿ�߸� �� �� �ִ� �ڸ� false�� �̹� ���� �ִ� �ڸ�
    public bool Check_Can_Add(Player player, int row, int col)
    {
        // �̹� �� �ڸ��� ���� �ִ��� Ȯ��
        if (Black_Chip.Exists(chip => chip.Row == row && chip.Col == col) ||
            White_Chip.Exists(chip => chip.Row == row && chip.Col == col))
        {
            return false;
        }

        return true;
    }

    //false ��ȯ�� ��쿡�� �����
    public bool Check_SamSam(Chip proposedChip)
    {
        int samCount = 0;

        // �� ���� Ȯ��
        if (Check_ThreeInARow(Black_Chip, proposedChip, 1, 0)) samCount++;  // ����
        if (Check_ThreeInARow(Black_Chip, proposedChip, 0, 1)) samCount++;  // ����
        if (Check_ThreeInARow(Black_Chip, proposedChip, 1, 1)) samCount++;  // ������ �밢��
        if (Check_ThreeInARow(Black_Chip, proposedChip, 1, -1)) samCount++; // ����� �밢��

        return samCount < 2;
    }

    #endregion
    //==============================================================================================================================================

    private bool Check_ThreeInARow(List<Chip> chips, Chip proposedChip, int dx, int dy)
    {
        int count = 1;

        // ������ üũ
        count += CountChipsInDirection(chips, proposedChip, dx, dy);
        // ������ üũ
        count += CountChipsInDirection(chips, proposedChip, -dx, -dy);

        // count�� 3���� ��츸 true ��ȯ
        return count == 3;
    }


}
