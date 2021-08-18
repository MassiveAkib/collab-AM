using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GunAnimation : MonoBehaviour
{
    private Sequence moveSequence = null;
    private Sequence fireSequence = null;
    private Sequence continuousFireSequence = null;
    private Sequence reloadSequence = null;

    private bool isMoveEnd = true;
    private bool setMoveEnd = true;
    private bool isFire = false;
    private bool isReload = false;

    public Camera playerCamera = null;
    public GameObject magazine = null;

    private PlayerMove playerMove = null;

    public Vector3 cameraShakePosition = new Vector3(0f, 0.1f, 0f);

    private void Awake()
    {
        if (playerCamera == null)
        {
            Debug.LogError("Camera가 없습니다.");
        }

        if (magazine == null)
        {
            Debug.LogError("magazin이 없습니다.");
        }

        playerMove = FindObjectOfType<PlayerMove>();

        if (playerMove == null)
        {
            Debug.LogError("playerMove가 없습니다.");
        }

        moveSequence = DOTween.Sequence();
        fireSequence = DOTween.Sequence();
        continuousFireSequence = DOTween.Sequence();
        reloadSequence = DOTween.Sequence();

        CreateMoveSequence();
        CreateFireSequence(fireSequence, -10f, 0.1f);
        CreateFireSequence(continuousFireSequence, -5f, 0.05f);
        CreateReloadSequence();
    }

    private void CreateMoveSequence()
    {
        moveSequence.Append(transform.DOLocalRotate(new Vector3(-2f, 0f, 0f), 0.5f).SetRelative());
        moveSequence.AppendInterval(0.1f);
        moveSequence.Append(transform.DOLocalRotate(new Vector3(2f, 0f, 0f), 0.5f).SetRelative());
        moveSequence.AppendInterval(0.1f);

        moveSequence.OnComplete(null);
        moveSequence.SetAutoKill(false);
        moveSequence.Pause();
    }

    private void CreateFireSequence(Sequence sequence, float xValue, float delay)
    {
        sequence.Append(transform.DOLocalRotate(new Vector3(xValue, 0f, 0f), 0.1f).SetRelative());
        sequence.AppendInterval(delay / 2);
        sequence.Append(transform.DOLocalRotate(new Vector3(-xValue, 0f, 0f), 0.5f).SetRelative());
        sequence.AppendInterval(delay);

        sequence.OnComplete(() => {
            isFire = false;
        });
        sequence.SetAutoKill(false);
        sequence.Pause();
    }

    private void CreateReloadSequence()
    {
        reloadSequence.Append(magazine.transform.DOLocalMoveY(-10, 0.5f)).SetRelative();
        reloadSequence.AppendInterval(0.5f);
        reloadSequence.Append(magazine.transform.DOLocalMoveY(10, 0.5f)).SetRelative();
        reloadSequence.AppendInterval(0.5f);

        reloadSequence.OnComplete(() => {
            isReload = false;
        });
        reloadSequence.SetAutoKill(false);
        reloadSequence.Pause();
    }

    public void MoveStart()
    {
        if (!isMoveEnd || !isMoveEnd || isFire || isReload) return;

        moveSequence.Restart();
        moveSequence.OnComplete(() => moveSequence.Restart());

        isMoveEnd = false;
        setMoveEnd = false;
    }

    public void MoveEnd()
    {
        if (setMoveEnd) return;

        setMoveEnd = true;
        moveSequence.OnComplete(() => isMoveEnd = true);
    }

    public void FireStart()
    {
        isFire = true;
        MoveEnd();
        moveSequence.Complete();
        fireSequence.Restart();

        playerMove.CameraRotation(0.5f);
        playerCamera.transform.DOShakePosition(0.1f, cameraShakePosition * 2, 1);
    }
    
    public void ContinuousFireStart()
    {
        isFire = true;
        MoveEnd();
        moveSequence.Complete();
        continuousFireSequence.Restart();

        playerMove.CameraRotation(0.25f);
        playerCamera.transform.DOShakePosition(0.05f, cameraShakePosition, 1);
    }

    public void ReloadStart()
    {
        if (isReload) return;

        isReload = true;
        MoveEnd();
        moveSequence.Complete();
        fireSequence.Complete();
        continuousFireSequence.Complete();
        reloadSequence.Restart();
    }

    public void AnimationEnd()
    {
        moveSequence.Kill();
        fireSequence.Kill();
        continuousFireSequence.Kill();
        reloadSequence.Kill();
    }
}
