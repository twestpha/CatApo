using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Strings {
    public enum LocalizedString {
        DefaultString,
        TestString,
    }

    static string[] stringDatabase = new string[]{
        "Default String",
        "Test string. Has a newline.\nAnd a \ttab character. Also, here's some extended ascii: waßermelone.",
    };

    public static string GetString(LocalizedString stringEnum){
        return stringDatabase[(int)stringEnum];
    }
}
