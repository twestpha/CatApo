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
        TestString,
        TestString2,
    }

    static string[] stringDatabase = new string[]{
        "Default String",
        "Test string. Has a newline.\nAnd a \ttab character. Also, here's some extended ascii: waßermelone. τΦΘΩδ∞φε∩",
        "Another, different test string...",
    };

    public static string GetString(LocalizedString stringEnum){
        return stringDatabase[(int)stringEnum];
    }
}
