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
    }
}
