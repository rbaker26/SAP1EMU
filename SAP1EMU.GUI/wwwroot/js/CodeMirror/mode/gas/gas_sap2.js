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

    CodeMirror.defineMode("gas_sap2", function (_config, parserConfig) {
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
        
        const sap2Variables = [
            'add b',
            'add c',
            'ana b',
            'ana c',
            'ani',
            'call',
            'cma',
            'dcr a',
            'dcr b',
            'dcr c',
            'inr a',
            'inr b',
            'inr c',
            'lda',
            'mov a,b',
            'mov a,c',
            'mov b,a',
            'mov b,c',
            'mov c,a',
            'mov c,b',
            'ora b',
            'ora c',
            'ori',
            'ral',
            'rar',
            'sta',
            'sub b',
            'sub c',
            'xra b',
            'xra c',
            'xri'
        ];
        
        const sap2Keywords = [
            'jm',
            'jmp',
            'jnz',
            'jz',
        ];
        
        const sap2Builtin = [
            'in',
            'out',
            'hlt',
            'ret'
        ];
        
        function x86(_parserConfig) {
            lineCommentStartSymbol = "#";

            for (const variable of sap2Variables) {
                registers[variable] = "variable-3";
            }

            for (const variable of sap2Keywords) {
                registers[variable] = "keyword";
            }

            for (const variable of sap2Builtin) {
                registers[variable] = "builtin";
            }
            
            console.log(registers);
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
                    return "bracket";
                }

                if (ch === '}') {
                    return "bracket";
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
                    // I originally wanted to colorize ..., but the dots were so small you couldn't even tell
                    // To change the color in the future, edit the return in IF-statement below
                    if (ch === "." && stream.eat(".") && stream.eat(".")) {
                        return "variable";
                    }
                    stream.eatWhile(/\w/);
                    cur = stream.current().toLowerCase();
                    
                    for(const variable of sap2Variables) {
                        if(variable.includes(cur) && variable.includes(' ')) {
                            cur = stream.string.toLowerCase().trim();
                            break;
                        }
                    }
                    
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