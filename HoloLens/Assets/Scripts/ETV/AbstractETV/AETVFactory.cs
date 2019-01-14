/*
Vinked Views
Copyright(C) 2018  Georg Eckert

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using Model;
using UnityEngine;

namespace ETV
{
    public abstract class AETVFactory : MonoBehaviour
    {
        public GameObject ETVAnchorPrefab;

        public abstract AETVSingleAxis  CreateSingleAxis(DataSet data, string attributeName, bool isMetaVis=false);
        public abstract AETVPCP         CreatePCP(DataSet data, string[] attIDs, bool isMetaVis = false);
        public abstract AETVLineChart   CreateLineChart(DataSet data, string attributeNameA, string attributeNameB, bool isMetaVis = false);
        public abstract AETVBarChart    CreateBarChart(DataSet data, string attributeName, bool isMetaVis = false);
        public abstract AETVScatterPlot CreateScatterplot(DataSet data, string[] attIDs, bool isMetaVis = false);


        public GameObject PutETVOnAnchor(GameObject ETV)
        {
            var Anchor = Instantiate(ETVAnchorPrefab);
            Anchor.GetComponent<ETVAnchor>().PutETVintoAnchor(ETV);
            return Anchor;
        }
    }
}