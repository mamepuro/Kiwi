using System;
using System.Collections.Generic;
using System.Text;

namespace Kiwi
{
    /// <summary>
    /// ゲームの進行状況を表す
    /// </summary>
    enum GamePhase
    {
        /// <summary>
        /// 手札の花札を選択しているフェーズ
        /// </summary>
        SelectingHandCardPhase,
        /// <summary>
        /// 場の花札を選択しているフェーズ
        /// </summary>
        SelectingFieldCardPhase,
        /// <summary>
        /// チェックを行うフェーズ
        /// </summary>
        checkingPhase,
        /// <summary>
        /// カードを取得するフェーズ
        /// </summary>
        GettingCardsPhase,
        /// <summary>
        /// 花札を取った後山札の一番上のカードとフィールドのカードを選ぶフェーズ
        /// </summary>
        SelectingOneMoreCardPhase,
        /// <summary>
        /// 2回めのチェックを行うフェーズ
        /// </summary>
        SecondCheckingPhase,
        /// <summary>
        /// ターン主の交換を行うフェーズ
        /// </summary>
        SwappingTurnMasterPhase
    }
}
