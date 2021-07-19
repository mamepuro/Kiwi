using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using Altseed2;

namespace Kiwi
{
    class Player
    {
        /// <summary>
        /// 所属しているシーンのノード
        /// </summary>
        GameMainScene mainNode;
        /// <summary>
        /// 手札
        /// </summary>
        public List<Card> hand;
        /// <summary>
        /// 光札のうち取り札
        /// </summary>
        public List<Card> HikariCardCollection;
        /// <summary>
        /// 種札のうち取り札
        /// </summary>
        public List<Card> TaneCardCollection;
        /// <summary>
        /// 短冊札のうち取り札
        /// </summary>
        public List<Card> TanzakuCardCollection;
        /// <summary>
        /// カス札のうち取り札
        /// </summary>
        public List<Card> KasuCardCollection;
        /// <summary>
        /// プレイヤーの属性
        /// </summary>
        public PlayerAttribute _attribute;
        /// <summary>
        /// 取り札を配置するスペースの基準点(スペースの左上の数値)
        /// </summary>
        public Vector2F CardStockPosition { get; set; }
        /// <summary>
        /// 人間の取り札スペースの基準点
        /// </summary>
        private Vector2F humanStockStandartPos = new Vector2F(1020, 650);
        /// <summary>
        /// CPUの取り札スペースの基準点
        /// </summary>
        private Vector2F CPUStockStandartPos = new Vector2F(300, 380);

        public Player(GameMainScene scene, PlayerAttribute attribute)
        {
            _attribute = attribute;
            mainNode = scene;
            HikariCardCollection = new List<Card>();
            TaneCardCollection = new List<Card>();
            TanzakuCardCollection = new List<Card>();
            KasuCardCollection = new List<Card>();
            if(_attribute == PlayerAttribute.Human)
            {
                CardStockPosition = humanStockStandartPos;
            }
            else
            {
                CardStockPosition = CPUStockStandartPos;
            }
        }
    }
}
