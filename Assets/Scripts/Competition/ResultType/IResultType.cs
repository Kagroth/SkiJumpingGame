using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IResultType
{
    void ApplyPoints(List<WorldCupSkiJumperResult> classification, List<CompetitionResult> competitionResults);
}
