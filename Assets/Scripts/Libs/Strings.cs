using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// All printable alt codes
//     0123456789
//   1 ☺☻♥♦♣♠•◘○◙
//  11 ♂♀♪♫☼►◄↕‼¶
//  21 §▬↨↑↓→←↕↔▲
//  31 ▼ !"#$%&'(
//  41 )*+,-./012
//  51 3456789:;<
//  61 =>?@ABCDEF
//  71 GHIJKLMNOP
//  81 QRSTUVWXYZ
//  91 [\]^_`abcd
// 101 efghijklmn
// 111 opqrstuvwx
// 121 yz{|}~⌂Çüé
// 131 âäàåçêëèïî
// 141 ìÄÅÉæÆôöòû
// 151 ùÿÖÜ¢£¥₧ƒá
// 161 íóúñÑªº¿⌐¬
// 171 ½¼¡«»░▒▓│╛
// 191 ┐└┴┬├─┼╞╟╚
// 201 ╔╩╦╠═╬╧╨╤╥
// 211 ╙╘╒╓╫╪┘┌█▄
// 221 ▌▐▀αßΓπΣσµ
// 231 τΦΘΩδ∞φε∩≡
// 241 ±≥≤⌠⌡÷≈°∙·
// 251 √ⁿ²■ 

public static class Strings {
    public enum LocalizedString {
        DefaultString,
        // Desolation
        Des_HistoryOfCastle1,
        Des_HistoryOfCastle2,
        Des_HistoryOfCastle3,
        Des_HistoryOfCastle4,
    }

    static string[] stringDatabase = new string[]{
        "Default String",
        // Desolation
        "History of Caer Tanisan, season of the Jaguar\nDuring routine patrols in the Osin desert to the east, soldiers reported strange shifts in the sand, as if something we moving far below. The men are growing strange from isolation, I fear.",
        "History of Caer Tanisan, season of the Owl\nThere have been reports of strange noises in the night, of trinkets missing and food stolen. No soldiers have yet confessed, but I remain wary.",
        "History of Caer Tanisan, season of the Manticore\nAt last, rain! Praise be to Shano, Keeper of the Heavens. Thus ends a too long and thirsty year. I think it odd, last years supply should have lasted long into next season. Thirsty is the desert, forsooth.",
    };

    public static string GetString(LocalizedString stringEnum){
        return stringDatabase[(int)stringEnum];
    }
}
