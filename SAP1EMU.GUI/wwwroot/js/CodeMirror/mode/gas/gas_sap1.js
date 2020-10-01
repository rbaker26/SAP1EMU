// CodeMirror, copyright (c) by Marijn Haverbeke and others
// Distributed under an MIT license: https://codemirror.net/LICENSE

(function (mod) {
    if (typeof exports == "object" && typeof module == "object") // CommonJS
        mod(require("../../lib/codemirror"));
    else if (typeof define == "function" && define.amd) // AMD
        define(["../../lib/codemirror"], mod);
    else // Plain browser env
        mod(CodeMirror);
})(function (CodeMirror) {
    "use strict";

    CodeMirror.defineMode("gas_sap1", function (_config, parserConfig) {
        'use strict';

        // If an architecture is specified, its initialization function may
        // populate this array with custom parsing functions which will be
        // tried in the event that the standard functions do not find a match.
        var custom = [];

        // The symbol used to start a line comment changes based on the target
        // architecture.
        // If no architecture is pased in "parserConfig" then only multiline
        // comments will have syntax support.
        var lineCommentStartSymbol = "";

        // These directives are architecture independent.
        // Machine specific directives should go in their respective
        // architecture initialization function.
        // Reference:
        // http://sourceware.org/binutils/docs/as/Pseudo-Ops.html#Pseudo-Ops
        var directives = {
            ".null": "variable",
        };

        var registers = {};

        function x86(_parserConfig) {
            lineCommentStartSymbol = "#";

            // The Memory Referenced Instructions
            registers.lda = "variable-3";
            registers.ldb = "variable-3";
            registers.add = "variable-3";
            registers.sub = "variable-3";
            registers.sta = "variable-3";
            registers.ldi = "variable-3";

            // The Value Regerenced Instructions
            registers.jmp = "keyword";
            registers.jeq = "keyword";
            registers.jnq = "keyword";
            registers.jlt = "keyword";
            registers.jgt = "keyword";
            registers.jic = "keyword";

            // The Null Referenced Intstructions
            registers.out = "builtin";
            registers.hlt = "builtin";
        }

        // Not Used
        function armv6(_parserConfig) {
            lineCommentStartSymbol = "@";
            directives.syntax = "builtin";

            custom.push(function (ch, stream) {
                if (ch === '#') {
                    stream.eatWhile(/\w/);
                    return "number";
                }
            });
        }

        var arch = (parserConfig.architecture || "x86").toLowerCase();
        if (arch === "x86") {
            x86(parserConfig);
        } else if (arch === "arm" || arch === "armv6") {
            // Not used
            armv6(parserConfig);
        }

        function nextUntilUnescaped(stream, end) {
            var escaped = false, next;
            while ((next = stream.next()) != null) {
                if (next === end && !escaped) {
                    return false;
                }
                escaped = !escaped && next === "\\";
            }
            return escaped;
        }

        function clikeComment(stream, state) {
            var maybeEnd = false, ch;
            while ((ch = stream.next()) != null) {
                if (ch === "/" && maybeEnd) {
                    state.tokenize = null;
                    break;
                }
                maybeEnd = (ch === "*");
            }
            return "variable";
        }

        return {
            startState: function () {
                return {
                    tokenize: null
                };
            },

            token: function (stream, state) {
                if (state.tokenize) {
                    return state.tokenize(stream, state);
                }

                if (stream.eatSpace()) {
                    return null;
                }

                var style, cur, ch = stream.next();

                //if (ch === "/") {
                //    if (stream.eat("*")) {
                //        state.tokenize = clikeComment;
                //        return clikeComment(stream, state);
                //    }
                //}

                if (ch === lineCommentStartSymbol) {
                    stream.skipToEnd();
                    return "comment";
                }

                if (ch === '"') {
                    nextUntilUnescaped(stream, '"');
                    return "string";
                }

                if (ch === '=') {
                    stream.eatWhile(/\w/);
                    return "tag";
                }

                if (ch === '{') {
                    return "braket";
                }

                if (ch === '}') {
                    return "braket";
                }

                if (/\d/.test(ch)) {
                    if (ch === "0" && stream.eat("x")) {
                        stream.eatWhile(/[0-9a-fA-F]/);
                        return "variable-2";
                    }
                    stream.eatWhile(/\d/);
                    return "variable-2";
                }

                if (/\w/.test(ch)) {
                    // Checks for ... macro
                    // I orginally wanted to colorize ..., but the dots were so small you couldn't even tell
                    // To change the color in the future, edit the return in IF-statment below
                    if (ch === "." && stream.eat(".") && stream.eat(".")) {
                        return "variable";
                    }
                    stream.eatWhile(/\w/);
                    cur = stream.current().toLowerCase();
                    style = registers[cur];
                    return style || null;
                }

                for (var i = 0; i < custom.length; i++) {
                    style = custom[i](ch, stream, state);
                    if (style) {
                        return style;
                    }
                }
            },

            lineComment: lineCommentStartSymbol,
            blockCommentStart: "/*",
            blockCommentEnd: "*/"
        };
    });
});