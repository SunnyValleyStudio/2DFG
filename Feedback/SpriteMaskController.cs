using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.Feedback
{
    [RequireComponent(typeof(Collider2D))]
    public class SpriteMaskController : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _playerSpriteRenderer;
        [SerializeField]
        private SpriteMask _spriteMask;

        private List<SpriteRenderer> _otherRenderers = new();

        public bool _checking = false;

        private void Awake()
        {
            GetComponent<Collider2D>().isTrigger = true;
        }

        private void Update()
        {
            if(_checking)
            {
                foreach(SpriteRenderer renderer in _otherRenderers)
                {
                    if(_playerSpriteRenderer.sortingLayerName == renderer.sortingLayerName
                        && _playerSpriteRenderer.sortingOrder <= renderer.sortingOrder
                        && _playerSpriteRenderer.transform.position.y  
                        > renderer.transform.position.y)
                    {
                        _spriteMask.enabled = true;
                        _playerSpriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                        return;
                    }
                    else
                    {
                        _spriteMask.enabled = false;
                        _playerSpriteRenderer.maskInteraction = SpriteMaskInteraction.None;
                    }
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.isTrigger == false) 
                return;
            SpriteRenderer spriteRenderer = collision.GetComponent<SpriteRenderer>();
            if(spriteRenderer != null)
            {
                _otherRenderers.Add(spriteRenderer);
                _checking = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.isTrigger == false)
                return;
            SpriteRenderer spriteRenderer = collision.GetComponent<SpriteRenderer>();
            if(spriteRenderer != null )
            {
                _otherRenderers.Remove(spriteRenderer);
                if(_otherRenderers.Count <=0)
                {
                    _checking = false;
                    _spriteMask.enabled = false;
                    _playerSpriteRenderer.maskInteraction = SpriteMaskInteraction.None;
                }
            }
        }
    }
}
