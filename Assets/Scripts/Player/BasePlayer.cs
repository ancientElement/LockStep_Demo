using System;
using AE_BEPUPhysics_Addition;
using AE_BEPUPhysics_Addition.Interface;
using NetGameRunning;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;

namespace LockStep_Demo
{
    public class BasePlayer
    {
        public enum STATEENUM
        {
            idle,
            move
        }

        float m_velocity;
        STATEENUM m_state;
        GameObject m_go;
        Vector3 m_targetPos;

        public BasePlayer(STATEENUM state, GameObject gameObject, float velocity)
        {
            m_state = state;
            m_go = gameObject;
            m_velocity = velocity;
            m_targetPos = m_go.transform.position;
        }

        public void OnFixedUpdate(float delta)
        {
            // m_go.transform.position = Vector3.MoveTowards(m_go.transform.position, m_targetPos, delta * m_velocity);
        }

        public void OnLogicUpdate(float delta, PlayerInputData playerInput)
        {
            if (playerInput.JoyX != 0 ||
                playerInput.JoyY != 0)
            {
                m_state = STATEENUM.move;
            }
            else
            {
                m_state = STATEENUM.idle;
            }

            switch (m_state)
            {
                case STATEENUM.idle:
                    IdleUpdate(delta);
                    break;
                case STATEENUM.move:
                    MoveUpdate(delta, playerInput);
                    break;
            }
        }

        protected virtual void MoveUpdate(float delta, PlayerInputData playerInput)
        {
            Move(delta, playerInput);
        }

        protected virtual void IdleUpdate(float delta)
        {
            // m_targetPos = m_go.transform.position;
            var body = m_go.GetComponent<BaseVolumnBaseCollider>();
            var velocity = new Vector3(0, body.GetVelocity().y, 0);
            body.SetVeolicty(velocity);
        }

        public void Move(float delta, PlayerInputData playerInput)
        {
            var direction = new Vector3(playerInput.JoyX, 0, playerInput.JoyY);
            var tempVelocity = direction * m_velocity;
            // m_targetPos = m_go.transform.position + tempVelocity * delta;

            var body = m_go.GetComponent<BaseVolumnBaseCollider>();
            var velocity = body.GetVelocity();
            velocity.x = tempVelocity.x;
            velocity.z = tempVelocity.z;
            body.SetVeolicty(velocity);
        }
    }
}