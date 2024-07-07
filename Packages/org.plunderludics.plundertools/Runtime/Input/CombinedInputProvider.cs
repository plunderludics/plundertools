// [Work in progress]
// Merge inputs from multiple providers
// Key inputs are concatenated, axis inputs are summed

using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHawk;

namespace Plundertools {

public class CombinedInputProvider : InputProvider
{
    [SerializeField] List<InputProvider> providers;

    public override Dictionary<string, int> AxisValuesForFrame()
    {
        return providers.SelectMany(p => p.AxisValuesForFrame())
                        .ToLookup(pair => pair.Key, pair => pair.Value)
                        .ToDictionary(group => group.Key, group => group.Sum());
    }

    public override List<InputEvent> InputForFrame()
    {
        return providers.SelectMany(p => p.InputForFrame()).ToList();
    }
}

}