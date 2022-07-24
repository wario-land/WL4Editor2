using WL4EditorCore.Exception;
using static WL4EditorCore.Util.Constants;

namespace WL4EditorCore.Util
{
    // Member functions for enums
    public static class ConstantExtensions
    {
        public static string ToString(this LayerScrollingType a)
        {
            switch (a)
            {
                case LayerScrollingType.NoScrolling:       return "No scrolling";
                case LayerScrollingType.HSpeedHalf:        return "H speed ½ of BG1, V fixed";
                case LayerScrollingType.VSpeedHalf:        return "V speed ½ of BG1, H fixed";
                case LayerScrollingType.HVSpeedHalf:       return "H and V speed ½ of BG1";
                case LayerScrollingType.HSyncVSpeedHalf:   return "H synced wih BG1, V speed ½ of BG1";
                case LayerScrollingType.VSyncHSpeedHalf:   return "V synced wih BG1, H speed ½ of BG1";
                case LayerScrollingType.HVSync:            return "H and V synced wih BG1";
                case LayerScrollingType.TopHalfAutoscroll: return "Upper half autoscroll to the left (⅛ speed of BG1)";
            }
            throw new InternalException("Invalid enum passed to extension method WL4EditorCore.Util.ConstantExtensions.ToString(this LayerScrollingType)");
        }

        public static string ToString(this LayerSpecialEffect a)
        {
            switch (a)
            {
                case LayerSpecialEffect.NoEffect:            return "No raster type effect";
                case LayerSpecialEffect.Layer3WaterEffect1:  return "Layer 3 water effect 1";
                case LayerSpecialEffect.Layer3WaterEffect2:  return "Layer 3 water effect 2";
                case LayerSpecialEffect.Layer0FogEffect:     return "Layer 0 fog effect";
                case LayerSpecialEffect.Layer3FireEffect1:   return "Layer 3 fire effect 1";
                case LayerSpecialEffect.Layer3FireEffect2:   return "Layer 3 fire effect 2";
                case LayerSpecialEffect.Layer3HDMAUpperHalf: return "Layer 3 top half ⅛ scroll, bottom half fixed";
                case LayerSpecialEffect.Layer3HDMALowerHalf: return "Layer 3 bottom half ⅛ scroll, top half fixed";
                case LayerSpecialEffect.AlphaFireEffect1:    return "Alpha fire effect 1";
                case LayerSpecialEffect.AlphaFireEffect2:    return "Alpha fire effect 2";
            }
            throw new InternalException("Invalid enum passed to extension method WL4EditorCore.Util.ConstantExtensions.ToString(this LayerSpecialEffect)");
        }

        public static string ToString(this LayerPriority a)
        {
            switch (a)
            {
                case LayerPriority.Order_0123:    return "L0 (Top) > L1 > L2 > L3";
                case LayerPriority.Order_1023:    return "L1 (Top) > L0 > L2 > L3";
                case LayerPriority.Order_1023_SP: return "L1 (Top) > L0 > L2 > L3, Sprite Priority+";
                case LayerPriority.Order_1203_SP: return "L1 (Top) > L2 > L0 > L3, Sprite Priority+";
            }
            throw new InternalException("Invalid enum passed to extension method WL4EditorCore.Util.ConstantExtensions.ToString(this LayerPriority)");
        }

        public static int GetLayerPriority(this LayerPriority a, int layer)
        {
            if (!Enum.IsDefined(a))
            {
                throw new InternalException("Invalid enum passed to extension method WL4EditorCore.Util.ConstantExtensions.GetLayerPriority(this LayerPriority, int)");
            }
            if (layer < 0 || layer >= 4)
            {
                throw new InternalException("Invalid layer value passed to extension method WL4EditorCore.Util.ConstantExtensions.GetLayerPriority(this LayerPriority, int)");
            }
            int[][] _pri =
            {
                new int[] { 0, 1, 2, 3 },
                new int[] { 1, 0, 2, 3 },
                new int[] { 1, 0, 2, 3 },
                new int[] { 2, 0, 1, 3 },
            };
            return _pri[(int)a][layer];
        }

        public static string ToString(this AlphaBlending a)
        {
            switch (a)
            {
                case AlphaBlending.NoAlphaBlending: return "No Alpha Blending";
                case AlphaBlending.EVA7_EVB16:      return "EVA 44%, EVB 100%";
                case AlphaBlending.EVA10_EVB16:     return "EVA 63%, EVB 100%";
                case AlphaBlending.EVA13_EVB16:     return "EVA 81%, EVB 100%";
                case AlphaBlending.EVA16_EVB16:     return "EVA 100%, EVB 100%";
                case AlphaBlending.EVA16_EVB0:      return "EVA 100%, EVB 0%";
                case AlphaBlending.EVA13_EVB3:      return "EVA 81%, EVB 19%";
                case AlphaBlending.EVA10_EVB6:      return "EVA 63%, EVB 37%";
                case AlphaBlending.EVA7_EVB9:       return "EVA 44%, EVB 56%";
                case AlphaBlending.EVA5_EVB11:      return "EVA 31%, EVB 68%";
                case AlphaBlending.EVA3_EVB13:      return "EVA 19%, EVB 81%";
                case AlphaBlending.EVA0_EVB16:      return "EVA 00%, EVB 100%";
            }
            throw new InternalException("Invalid enum passed to extension method WL4EditorCore.Util.ConstantExtensions.ToString(this AlphaBlending)");
        }

        public static int GetEVA(this AlphaBlending a)
        {
            if (!Enum.IsDefined(a) || a == AlphaBlending.NoAlphaBlending)
            {
                throw new InternalException("Invalid enum passed to extension method WL4EditorCore.Util.ConstantExtensions.GetEVA(this LayerPriority)");
            }
            int[] _eva = { 7, 10, 13, 16, 16, 13, 10, 7, 5, 3, 0 };
            return _eva[(int)a - 1];
        }

        public static int GetEVB(this AlphaBlending a)
        {
            if (!Enum.IsDefined(a) || a == AlphaBlending.NoAlphaBlending)
            {
                throw new InternalException("Invalid enum passed to extension method WL4EditorCore.Util.ConstantExtensions.GetEVB(this LayerPriority)");
            }
            int[] _evb = { 16, 16, 16, 16, 0, 3, 6, 9, 11, 13, 16 };
            return _evb[(int)a - 1];
        }
    }
}
