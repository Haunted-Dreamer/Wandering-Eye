﻿using UnityEngine;
using System.Collections;

//TODO: Pushes and Moves don't reset through UI callbacks when restarting level or checkpoints. Fix this.
//TODO: Allow only one checkpoint per level unless restarted.

[System.Serializable]
public class CheckpointController
{
    private int _StoredMoves = 0;
    private int _StoredPushes = 0;

    private Containers _Containers = null;
    private LevelManager _LevelManager = null;

    public void Init (Containers containers, LevelManager levelManager)
    {
        _Containers = containers;
        _LevelManager = levelManager;
    }

    public void GetInput ()
    {
        if (Input.GetKeyDown (KeyCode.R) || Input.GetButtonDown("Reset Level"))
            ResetCurrentLevel ();

        if (Input.GetKeyDown (KeyCode.E) || Input.GetButtonDown("Store Checkpoint"))
            StoreCheckpoint ();

        if (Input.GetKeyDown (KeyCode.Q) || Input.GetButtonDown("Restore Checkpoint"))
            RestoreCheckpoint ();
    }

    public void StoreStartPositions ()
    {
        _Containers.StartingPositions = new Vector3[_Containers.Bricks.Count + 1];
        _Containers.CheckpointPositions = new Vector3[_Containers.Bricks.Count + 1];

        for (int i = 0; i < _Containers.Bricks.Count; i++)
            _Containers.StartingPositions[i] = _Containers.Bricks[i].position;

        _Containers.StartingPositions[_Containers.StartingPositions.Length - 1] = _Containers.Player.transform.position;

        // Store an intial checkpoint at the start of the game.
        StoreCheckpoint ();
    }

    private void ResetCurrentLevel ()
    {
        for (int i = 0; i < _Containers.Bricks.Count; i++)
            _Containers.Bricks[i].position = _Containers.StartingPositions[i];

        _Containers.Player.transform.position = _Containers.StartingPositions[_Containers.StartingPositions.Length - 1];

        _LevelManager.Pushes = 0;
        _LevelManager.Moves = 0;
    }

    private void StoreCheckpoint ()
    {
        for (int i = 0; i < _Containers.Bricks.Count; i++)
            _Containers.CheckpointPositions[i] = _Containers.Bricks[i].position;

        _Containers.CheckpointPositions[_Containers.CheckpointPositions.Length - 1] = _Containers.Player.transform.position;

        //TODO: Store the current number of pushes and moves.
        _StoredPushes = _LevelManager.Pushes;
        _StoredMoves = _LevelManager.Moves;
    }

    private void RestoreCheckpoint ()
    {
        for (int i = 0; i < _Containers.Bricks.Count; i++)
            _Containers.Bricks[i].position = _Containers.CheckpointPositions[i];

        _Containers.Player.transform.position = _Containers.CheckpointPositions[_Containers.CheckpointPositions.Length - 1];

        //TODO: Restore the number of pushes and moves.
        _LevelManager.Pushes = _StoredPushes;
        _LevelManager.Moves = _StoredMoves;
    }
}
