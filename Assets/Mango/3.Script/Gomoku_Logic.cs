using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gomoku_Logic : MonoBehaviour
{
    // 이미 둔것들 
    private List<Chip> Black_Chip = new List<Chip>();
    private List<Chip> White_Chip = new List<Chip>();

    [SerializeField] private GameObject Chip_Pivot;

    private void Awake()
    {
        Chip_Pivot = GameObject.Find("Chip_Pivot");
        int index = 0;
        for (int i = 0; i < 19; i++)
        {
            for (int j = 0; j < 19; j++)
            {
                if (Chip_Pivot.transform.GetChild(index).TryGetComponent(out Chip chip))
                {
                    chip.Row = j;
                    chip.Col = i;
                    index++;
                }
            }
        }
    }

    // 착수 이후 결과값 검출
    private void Check_Chip(Player player, Chip lastChip)
    {
        List<Chip> playerChips = player.MyColor.Equals(0) ? Black_Chip : White_Chip;

        // 바둑알이 5개가 안되는 경우
        if (playerChips.Count < 5)
        {
            return;
        }

        // 네 가지 방향을 체크
        if (Check_Direction(playerChips, lastChip, 1, 0) ||  // 가로
            Check_Direction(playerChips, lastChip, 0, 1) ||  // 세로
            Check_Direction(playerChips, lastChip, 1, 1) ||  // 우하향 대각선
            Check_Direction(playerChips, lastChip, 1, -1))   // 우상향 대각선
        {
            EndGame(player, playerChips.ToArray());
        }
    }

    // 특정 방향으로 연속된 돌의 개수 확인
    private bool Check_Direction(List<Chip> chips, Chip lastChip, int dx, int dy)
    {
        int count = 1;

        // 정방향 체크
        count += CountChipsInDirection(chips, lastChip, dx, dy);
        // 역방향 체크
        count += CountChipsInDirection(chips, lastChip, -dx, -dy);

        Debug.Log(count);
        return count >= 5;
    }

    // 주어진 방향으로 연속된 돌의 수 반환
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

    // 게임 종료 (추가 구현 필요)
    private void EndGame(Player player, Chip[] chips)
    {
        Debug.Log($"{player.color} 승리");
        // 게임 종료 로직 구현

    }

    //==============================================================================================================================================
    #region 정문님 확인 필요 메서드

    // 착수 시, 호출해야 하는 메서드
    public void AddChip(Chip chip, Player player)
    {
        if (player.MyColor.Equals(0))
        {
            Black_Chip.Add(chip);
        }
        else if(player.MyColor.Equals(1)) 
        {
            White_Chip.Add(chip);
        }

        // 돌을 추가한 후 즉시 오목 여부를 체크합니다.
        Check_Chip(player, chip);
    }

    //true가 반환되어야만 둘 수 있는 자리 false는 이미 돌이 있는 자리
    public bool Check_Can_Add(Player player, int row, int col)
    {
        // 이미 그 자리에 돌이 있는지 확인
        if (Black_Chip.Exists(chip => chip.Row == row && chip.Col == col) ||
            White_Chip.Exists(chip => chip.Row == row && chip.Col == col))
        {
            return false;
        }

        return true;
    }

    //false 반환될 경우에는 삼삼임
    public bool Check_SamSam(Chip proposedChip)
    {
        int samCount = 0;

        // 네 방향 확인
        if (Check_ThreeInARow(Black_Chip, proposedChip, 1, 0)) samCount++;  // 가로
        if (Check_ThreeInARow(Black_Chip, proposedChip, 0, 1)) samCount++;  // 세로
        if (Check_ThreeInARow(Black_Chip, proposedChip, 1, 1)) samCount++;  // 우하향 대각선
        if (Check_ThreeInARow(Black_Chip, proposedChip, 1, -1)) samCount++; // 우상향 대각선

        return samCount < 2;
    }

    #endregion
    //==============================================================================================================================================

    private bool Check_ThreeInARow(List<Chip> chips, Chip proposedChip, int dx, int dy)
    {
        int count = 1;

        // 정방향 체크
        count += CountChipsInDirection(chips, proposedChip, dx, dy);
        // 역방향 체크
        count += CountChipsInDirection(chips, proposedChip, -dx, -dy);

        // count가 3개일 경우만 true 반환
        return count == 3;
    }


}
