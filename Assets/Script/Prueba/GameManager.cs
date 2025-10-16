using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public float mismatchDelay = 0.8f; // tiempo mostrando cartas si son incorrectas
    bool _locked;                      // para evitar que toquen otras cartas
    Card _first, _second;

    public void OnCardRevealed(Card c)
    {
        if (_locked) { c.FlipDown(); return; }

        if (_first == null)
        {
            _first = c;
            return;
        }

        if (_second == null && c != _first)
        {
            _second = c;
            StartCoroutine(EvaluatePair());
        }
    }

    IEnumerator EvaluatePair()
    {
        _locked = true;

        if (_first.pairId == _second.pairId)
        {
            // ¡Match!
            yield return new WaitForSeconds(0.15f);
            _first.LockAsMatched();
            _second.LockAsMatched();
        }
        else
        {
            // No coinciden ? dar vuelta de nuevo
            yield return new WaitForSeconds(mismatchDelay);
            _first.FlipDown();
            _second.FlipDown();
        }

        _first = null;
        _second = null;
        _locked = false;
    }
}
