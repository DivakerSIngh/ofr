function FastClick(e, t) {
    "use strict";

    function n(e, t) {
        return function () {
            return e.apply(t, arguments)
        }
    }
    var r;
    t = t || {}, this.trackingClick = !1, this.trackingClickStart = 0, this.targetElement = null, this.touchStartX = 0, this.touchStartY = 0, this.lastTouchIdentifier = 0, this.touchBoundary = t.touchBoundary || 10, this.layer = e, this.tapDelay = t.tapDelay || 200;
    if (FastClick.notNeeded(e)) return;
    var i = ["onMouse", "onClick", "onTouchStart", "onTouchMove", "onTouchEnd", "onTouchCancel"],
        s = this;
    for (var o = 0, u = i.length; o < u; o++) s[i[o]] = n(s[i[o]], s);
    deviceIsAndroid && (e.addEventListener("mouseover", this.onMouse, !0), e.addEventListener("mousedown", this.onMouse, !0), e.addEventListener("mouseup", this.onMouse, !0)), e.addEventListener("click", this.onClick, !0), e.addEventListener("touchstart", this.onTouchStart, !1), e.addEventListener("touchmove", this.onTouchMove, !1), e.addEventListener("touchend", this.onTouchEnd, !1), e.addEventListener("touchcancel", this.onTouchCancel, !1), Event.prototype.stopImmediatePropagation || (e.removeEventListener = function (t, n, r) {
        var i = Node.prototype.removeEventListener;
        t === "click" ? i.call(e, t, n.hijacked || n, r) : i.call(e, t, n, r)
    }, e.addEventListener = function (t, n, r) {
        var i = Node.prototype.addEventListener;
        t === "click" ? i.call(e, t, n.hijacked || (n.hijacked = function (e) {
            e.propagationStopped || n(e)
        }), r) : i.call(e, t, n, r)
    }), typeof e.onclick == "function" && (r = e.onclick, e.addEventListener("click", function (e) {
        r(e)
    }, !1), e.onclick = null)
}

function checkSpecialCharacters(e) {
    var t = new RegExp('^[0-9!#$%^&*()`~\\_\\+=<>.?/:;\\\\{}\\|\\[\\]\\"]'),
        n = String.fromCharCode(e.charCode ? e.charCode : e.which);
    if (t.test(n)) return e.preventDefault(), !1
}

function checkSpecialCharactersNonNumeric(e) {
    var t = new RegExp('^[!#$%^&*()`~\\_\\+=<>.?/:;\\\\{}\\|\\[\\]\\"]'),
        n = String.fromCharCode(e.charCode ? e.charCode : e.which);
    if (t.test(n)) return e.preventDefault(), !1
}

function getHotelCode(e, t) {
    var n;
    return $.ajax({
        url: WEBSITE_URL + "new_hlp/getHotelCode/?q=" + e,
        type: "GET",
        async: !1,
        success: function (t, r, i) {
            if (t) {
                obj = JSON.parse(t);
                var s = {
                    hotel: obj,
                    hotel_c_l: obj,
                    city: e,
                    country: e,
                    label: e,
                    value: e
                };
                n = obj
            }
        },
        error: function (e, t, n) { }
    }), n
}

function ghf_field_empty(e) {
    typeof e == "object" && ghf_Trim(e.value) == e.defaultValue && (e.value = "")
}

function ghf_field_restore(e) {
    typeof e == "object" && ghf_Trim(e.value) == "" && (e.value = e.defaultValue)
}

function ghf_LTrim(e) {
    var t = new String(" 	\n\r"),
        n = new String(e);
    if (t.indexOf(n.charAt(0)) != -1) {
        var r = 0,
            i = n.length;
        while (r < i && t.indexOf(n.charAt(r)) != -1) r++;
        n = n.substring(r, i)
    }
    return n
}

function ghf_RTrim(e) {
    var t = new String(" 	\n\r"),
        n = new String(e);
    if (t.indexOf(n.charAt(n.length - 1)) != -1) {
        var r = n.length - 1;
        while (r >= 0 && t.indexOf(n.charAt(r)) != -1) r -= 1;
        n = n.substring(0, r + 1)
    }
    return n
}

function ghf_Trim(e) {
    return ghf_RTrim(ghf_LTrim(e))
}

function validateEmail(e) {
    var t = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;
    return t.test(e) == 0 ? !1 : !0
}

function hideError_msgs(e) {
    var t = ["chf_error_name", "chf_error_email", "chf_error_comment", "chf_fdbck_succs"];
    for (var n = 0; n < t.length; n++) document.getElementById(t[n]) != null && document.getElementById(t[n]).innerHTML == "" && (e != null || e != "undefined" ? t[n] != e ? document.getElementById(t[n]).style.display = "none" : document.getElementById(e).style.display = "block" : document.getElementById(t[n]).style.display = "none")
}

function fdbck_Validate(e) {
    var t = e.elements,
        n = {
            count_increase: 0,
            set: function (e, t) {
                this[e] = t
            },
            get: function (e) {
                return this[e]
            }
        };
    for (var r = 0; r < t.length; r++) {
        if (t[r].name == "full_name") {
            if (t[r].value == "" || ghf_Trim(t[r].value).toLowerCase() == "name") n.count_increase = 1, n.set(t[r].name, t[r].value);
            t[r].className = ""
        } else if (t[r].name == "chf_email") t[r].value == "" || ghf_Trim(t[r].value).toLowerCase() == "email id" ? (n.count_increase = 2, n.set(t[r].name, t[r].value)) : t[r].value != "" && (validateEmail(t[r].value) || (n.count_increase = 2, n.set(t[r].name, t[r].value))), t[r].className = "";
        else if (t[r].name == "your_comment") {
            if (t[r].value == "" || ghf_Trim(t[r].value) == "Your Comments:") n.count_increase = 3, n.set(t[r].name, t[r].value);
            t[r].className = ""
        }
        document.getElementById("chf_error_name").innerHTML = "", document.getElementById("chf_error_email").innerHTML = "", document.getElementById("chf_error_comment").innerHTML = ""
    }
    if (n.count_increase > 0) {
        for (var i in n)
            if (typeof n[i] != "function" && typeof n[i] == "string" && n[i] != null) {
                var s = i;
                document.getElementById(s) != null && document.getElementById(s).className.indexOf("chf_error_flds") == -1 && (document.getElementById(s).className += " chf_error_flds", s == "full_name" ? document.getElementById("chf_error_name").innerHTML = "* Enter Name" : s == "chf_email" ? document.getElementById("chf_error_email").innerHTML = "* Enter a valid E-mail ID" : s == "your_comment" && (document.getElementById("chf_error_comment").innerHTML = "* Enter your Comments"))
            }
        return !1
    }
    return document.getElementById("chf_form_fields").style.display = "none", document.getElementById("chf_fdbck_succs").innerHTML = "Thanks for your valueable feedback", hideError_msgs("chf_fdbck_succs"), !1
}

function ghf_getNextSibling(e) {
    endBrother = e.nextSibling;
    while (endBrother.nodeType != 1) endBrother = endBrother.nextSibling;
    return endBrother
}

function ghf_slideToggle(e) {
    var t = "";
    e.className.indexOf("ghf_more_tab") != -1 ? t = document.getElementById("ghf_footer_center") : t = ghf_getNextSibling(e);
    var n = document.getElementById("textual"),
        r = document.getElementById("icons");
    t.style.display == "none" && r.className != null ? (t.style.display = "block", n = n.innerHTML = "Less", r.className = r.className.replace("chf_expnd_state", "chf_collpse_state")) : (t.style.display = "none", n = n.innerHTML = "More", r.className = r.className.replace("chf_collpse_state", "chf_expnd_state"))
} (function (e, t) {
    function i(t, n) {
        var r, i, o, u = t.nodeName.toLowerCase();
        return "area" === u ? (r = t.parentNode, i = r.name, !t.href || !i || r.nodeName.toLowerCase() !== "map" ? !1 : (o = e("img[usemap=#" + i + "]")[0], !!o && s(o))) : (/input|select|textarea|button|object/.test(u) ? !t.disabled : "a" === u ? t.href || n : n) && s(t)
    }

    function s(t) {
        return e.expr.filters.visible(t) && !e(t).parents().addBack().filter(function () {
            return e.css(this, "visibility") === "hidden"
        }).length
    }
    var n = 0,
        r = /^ui-id-\d+$/;
    e.ui = e.ui || {}, e.extend(e.ui, {
        version: "1.10.4",
        keyCode: {
            BACKSPACE: 8,
            COMMA: 188,
            DELETE: 46,
            DOWN: 40,
            END: 35,
            ENTER: 13,
            ESCAPE: 27,
            HOME: 36,
            LEFT: 37,
            NUMPAD_ADD: 107,
            NUMPAD_DECIMAL: 110,
            NUMPAD_DIVIDE: 111,
            NUMPAD_ENTER: 108,
            NUMPAD_MULTIPLY: 106,
            NUMPAD_SUBTRACT: 109,
            PAGE_DOWN: 34,
            PAGE_UP: 33,
            PERIOD: 190,
            RIGHT: 39,
            SPACE: 32,
            TAB: 9,
            UP: 38
        }
    }), e.fn.extend({
        focus: function (t) {
            return function (n, r) {
                return typeof n == "number" ? this.each(function () {
                    var t = this;
                    setTimeout(function () {
                        e(t).focus(), r && r.call(t)
                    }, n)
                }) : t.apply(this, arguments)
            }
        }(e.fn.focus),
        scrollParent: function () {
            var t;
            return e.ui.ie && /(static|relative)/.test(this.css("position")) || /absolute/.test(this.css("position")) ? t = this.parents().filter(function () {
                return /(relative|absolute|fixed)/.test(e.css(this, "position")) && /(auto|scroll)/.test(e.css(this, "overflow") + e.css(this, "overflow-y") + e.css(this, "overflow-x"))
            }).eq(0) : t = this.parents().filter(function () {
                return /(auto|scroll)/.test(e.css(this, "overflow") + e.css(this, "overflow-y") + e.css(this, "overflow-x"))
            }).eq(0), /fixed/.test(this.css("position")) || !t.length ? e(document) : t
        },
        zIndex: function (n) {
            if (n !== t) return this.css("zIndex", n);
            if (this.length) {
                var r = e(this[0]),
                    i, s;
                while (r.length && r[0] !== document) {
                    i = r.css("position");
                    if (i === "absolute" || i === "relative" || i === "fixed") {
                        s = parseInt(r.css("zIndex"), 10);
                        if (!isNaN(s) && s !== 0) return s
                    }
                    r = r.parent()
                }
            }
            return 0
        },
        uniqueId: function () {
            return this.each(function () {
                this.id || (this.id = "ui-id-" + ++n)
            })
        },
        removeUniqueId: function () {
            return this.each(function () {
                r.test(this.id) && e(this).removeAttr("id")
            })
        }
    }), e.extend(e.expr[":"], {
        data: e.expr.createPseudo ? e.expr.createPseudo(function (t) {
            return function (n) {
                return !!e.data(n, t)
            }
        }) : function (t, n, r) {
            return !!e.data(t, r[3])
        },
        focusable: function (t) {
            return i(t, !isNaN(e.attr(t, "tabindex")))
        },
        tabbable: function (t) {
            var n = e.attr(t, "tabindex"),
                r = isNaN(n);
            return (r || n >= 0) && i(t, !r)
        }
    }), e("<a>").outerWidth(1).jquery || e.each(["Width", "Height"], function (n, r) {
        function u(t, n, r, s) {
            return e.each(i, function () {
                n -= parseFloat(e.css(t, "padding" + this)) || 0, r && (n -= parseFloat(e.css(t, "border" + this + "Width")) || 0), s && (n -= parseFloat(e.css(t, "margin" + this)) || 0)
            }), n
        }
        var i = r === "Width" ? ["Left", "Right"] : ["Top", "Bottom"],
            s = r.toLowerCase(),
            o = {
                innerWidth: e.fn.innerWidth,
                innerHeight: e.fn.innerHeight,
                outerWidth: e.fn.outerWidth,
                outerHeight: e.fn.outerHeight
            };
        e.fn["inner" + r] = function (n) {
            return n === t ? o["inner" + r].call(this) : this.each(function () {
                e(this).css(s, u(this, n) + "px")
            })
        }, e.fn["outer" + r] = function (t, n) {
            return typeof t != "number" ? o["outer" + r].call(this, t) : this.each(function () {
                e(this).css(s, u(this, t, !0, n) + "px")
            })
        }
    }), e.fn.addBack || (e.fn.addBack = function (e) {
        return this.add(e == null ? this.prevObject : this.prevObject.filter(e))
    }), e("<a>").data("a-b", "a").removeData("a-b").data("a-b") && (e.fn.removeData = function (t) {
        return function (n) {
            return arguments.length ? t.call(this, e.camelCase(n)) : t.call(this)
        }
    }(e.fn.removeData)), e.ui.ie = !!/msie [\w.]+/.exec(navigator.userAgent.toLowerCase()), e.support.selectstart = "onselectstart" in document.createElement("div"), e.fn.extend({
        disableSelection: function () {
            return this.bind((e.support.selectstart ? "selectstart" : "mousedown") + ".ui-disableSelection", function (e) {
                e.preventDefault()
            })
        },
        enableSelection: function () {
            return this.unbind(".ui-disableSelection")
        }
    }), e.extend(e.ui, {
        plugin: {
            add: function (t, n, r) {
                var i, s = e.ui[t].prototype;
                for (i in r) s.plugins[i] = s.plugins[i] || [], s.plugins[i].push([n, r[i]])
            },
            call: function (e, t, n) {
                var r, i = e.plugins[t];
                if (!i || !e.element[0].parentNode || e.element[0].parentNode.nodeType === 11) return;
                for (r = 0; r < i.length; r++) e.options[i[r][0]] && i[r][1].apply(e.element, n)
            }
        },
        hasScroll: function (t, n) {
            if (e(t).css("overflow") === "hidden") return !1;
            var r = n && n === "left" ? "scrollLeft" : "scrollTop",
                i = !1;
            return t[r] > 0 ? !0 : (t[r] = 1, i = t[r] > 0, t[r] = 0, i)
        }
    })
})(jQuery),
function (e, t) {
    function i() {
        this._curInst = null, this._keyEvent = !1, this._disabledInputs = [], this._datepickerShowing = !1, this._inDialog = !1, this._mainDivId = "ui-datepicker-div", this._inlineClass = "ui-datepicker-inline", this._appendClass = "ui-datepicker-append", this._triggerClass = "ui-datepicker-trigger", this._dialogClass = "ui-datepicker-dialog", this._disableClass = "ui-datepicker-disabled", this._unselectableClass = "ui-datepicker-unselectable", this._currentClass = "ui-datepicker-current-day", this._dayOverClass = "ui-datepicker-days-cell-over", this.regional = [], this.regional[""] = {
            closeText: "Done",
            prevText: "Prev",
            nextText: "Next",
            currentText: "Today",
            monthNames: ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"],
            monthNamesShort: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
            dayNames: ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"],
            dayNamesShort: ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"],
            dayNamesMin: ["Su", "Mo", "Tu", "We", "Th", "Fr", "Sa"],
            weekHeader: "Wk",
            dateFormat: "mm/dd/yy",
            firstDay: 0,
            isRTL: !1,
            showMonthAfterYear: !1,
            yearSuffix: ""
        }, this.regional.hi = {
            closeText: "à¤¬à¤‚à¤¦",
            prevText: "à¤ªà¤¿à¤›à¤²à¤¾",
            nextText: "à¤…à¤—à¤²à¤¾",
            currentText: "à¤†à¤œ",
            monthNames: ["à¤œà¤¨à¤µà¤°à¥€ ", "à¤«à¤°à¤µà¤°à¥€", "à¤®à¤¾à¤°à¥à¤š", "à¤…à¤ªà¥à¤°à¥‡à¤²", "à¤®à¤ˆ", "à¤œà¥‚à¤¨", "à¤œà¥‚à¤²à¤¾à¤ˆ", "à¤…à¤—à¤¸à¥à¤¤ ", "à¤¸à¤¿à¤¤à¤®à¥à¤¬à¤°", "à¤…à¤•à¥à¤Ÿà¥‚à¤¬à¤°", "à¤¨à¤µà¤®à¥à¤¬à¤°", "à¤¦à¤¿à¤¸à¤®à¥à¤¬à¤°"],
            monthNamesShort: ["à¤œà¤¨", "à¤«à¤°", "à¤®à¤¾à¤°à¥à¤š", "à¤…à¤ªà¥à¤°à¥‡à¤²", "à¤®à¤ˆ", "à¤œà¥‚à¤¨", "à¤œà¥‚à¤²à¤¾à¤ˆ", "à¤…à¤—", "à¤¸à¤¿à¤¤", "à¤…à¤•à¥à¤Ÿ", "à¤¨à¤µ", "à¤¦à¤¿"],
            dayNames: ["à¤°à¤µà¤¿à¤µà¤¾à¤°", "à¤¸à¥‹à¤®à¤µà¤¾à¤°", "à¤®à¤‚à¤—à¤²à¤µà¤¾à¤°", "à¤¬à¥à¤§à¤µà¤¾à¤°", "à¤—à¥à¤°à¥à¤µà¤¾à¤°", "à¤¶à¥à¤•à¥à¤°à¤µà¤¾à¤°", "à¤¶à¤¨à¤¿à¤µà¤¾à¤°"],
            dayNamesShort: ["à¤°à¤µà¤¿", "à¤¸à¥‹à¤®", "à¤®à¤‚à¤—à¤²", "à¤¬à¥à¤§", "à¤—à¥à¤°à¥", "à¤¶à¥à¤•à¥à¤°", "à¤¶à¤¨à¤¿"],
            dayNamesMin: ["à¤°à¤µà¤¿", "à¤¸à¥‹à¤®", "à¤®à¤‚à¤—à¤²", "à¤¬à¥à¤§", "à¤—à¥à¤°à¥", "à¤¶à¥à¤•à¥à¤°", "à¤¶à¤¨à¤¿"],
            weekHeader: "à¤¹à¤«à¥à¤¤à¤¾",
            dateFormat: "dd/mm/yy",
            firstDay: 1,
            isRTL: !1,
            showMonthAfterYear: !1,
            yearSuffix: ""
        }, this.regional.ar = {
            closeText: "Ù‚Ø±ÙŠØ¨Ø§",
            prevText: "Ø³Ø§Ø¨Ù‚Ø©",
            nextText: "Ø§Ù„Ù…Ù‚Ø¨Ù„",
            currentText: "Ø§Ù„ÙŠÙˆÙ…",
            monthNames: ["ÙŠÙ†Ø§ÙŠØ±", "ÙØ¨Ø±Ø§ÙŠØ±", "Ù…Ø§Ø±Ø³", "Ø§Ø¨Ø±ÙŠÙ„", "Ù…Ø§ÙŠÙˆ", "ÙŠÙˆÙ†ÙŠÙˆ", "ÙŠÙˆÙ„ÙŠÙˆ", "Ø£ØºØ³Ø·Ø³", "Ø³Ø¨ØªÙ…Ø¨Ø±", "Ø£ÙƒØªÙˆØ¨Ø±", "Ù†ÙˆÙÙ…Ø¨Ø±", "Ø¯ÙŠØ³Ù…Ø¨Ø±"],
            monthNamesShort: ["ÙŠÙ†Ø§ÙŠØ±", "ÙØ¨Ø±Ø§ÙŠØ±", "Ù…Ø§Ø±Ø³", "Ø§Ø¨Ø±ÙŠÙ„", "Ù…Ø§ÙŠÙˆ", "ÙŠÙˆÙ†ÙŠÙˆ", "ÙŠÙˆÙ„ÙŠÙˆ", "Ø£ØºØ³Ø·Ø³", "Ø³Ø¨ØªÙ…Ø¨Ø±", "Ø£ÙƒØªÙˆØ¨Ø±", "Ù†ÙˆÙÙ…Ø¨Ø±", "Ø¯ÙŠØ³Ù…Ø¨Ø±"],
            dayNames: ["Ø§Ù„Ø£Ø­Ø¯", "Ø§Ù„Ø¥Ø«Ù†ÙŠÙ†", "Ø§Ù„Ø«Ù„Ø§Ø«Ø§Ø¡", "Ø§Ù„Ø£Ø±Ø¨Ø¹Ø§Ø¡", "Ø§Ù„Ø®Ù…ÙŠØ³", "Ø§Ù„Ø¬Ù…Ø¹Ø©", "Ø§Ù„Ø³Ø¨Øª"],
            dayNamesShort: ["Ø§Ù„Ø£Ø­Ø¯", "Ø§Ù„Ø¥Ø«Ù†ÙŠÙ†", "Ø§Ù„Ø«Ù„Ø§Ø«Ø§Ø¡", "Ø§Ù„Ø£Ø±Ø¨Ø¹Ø§Ø¡", "Ø§Ù„Ø®Ù…ÙŠØ³", "Ø§Ù„Ø¬Ù…Ø¹Ø©", "Ø§Ù„Ø³Ø¨Øª"],
            dayNamesMin: ["Ø§Ù„Ø£Ø­Ø¯", "Ø§Ù„Ø¥Ø«Ù†ÙŠÙ†", "Ø§Ù„Ø«Ù„Ø§Ø«Ø§Ø¡", "Ø§Ù„Ø£Ø±Ø¨Ø¹Ø§Ø¡", "Ø§Ù„Ø®Ù…ÙŠØ³", "Ø§Ù„Ø¬Ù…Ø¹Ø©", "Ø§Ù„Ø³Ø¨Øª"],
            weekHeader: "Ø§Ù„Ø£Ø³Ø¨ÙˆØ¹",
            dateFormat: "dd/mm/yy",
            firstDay: 1,
            isRTL: !0,
            showMonthAfterYear: !1,
            yearSuffix: ""
        }, this._defaults = {
            showOn: "focus",
            showAnim: "fadeIn",
            showOptions: {},
            defaultDate: null,
            appendText: "",
            buttonText: "...",
            buttonImage: "",
            buttonImageOnly: !1,
            hideIfNoPrevNext: !1,
            navigationAsDateFormat: !1,
            gotoCurrent: !1,
            changeMonth: !1,
            changeYear: !1,
            yearRange: "c-10:c+10",
            showOtherMonths: !1,
            selectOtherMonths: !1,
            showWeek: !1,
            calculateWeek: this.iso8601Week,
            shortYearCutoff: "+10",
            minDate: null,
            maxDate: null,
            duration: "fast",
            beforeShowDay: null,
            beforeShow: null,
            onSelect: null,
            onChangeMonthYear: null,
            onClose: null,
            numberOfMonths: 1,
            showCurrentAtPos: 0,
            stepMonths: 1,
            stepBigMonths: 12,
            altField: "",
            altFormat: "",
            constrainInput: !0,
            showButtonPanel: !1,
            autoSize: !1,
            disabled: !1
        }, e.extend(this._defaults, this.regional[""], this.regional[CAL_LANG]), this.dpDiv = s(e("<div id='" + this._mainDivId + "' class='ui-datepicker ui-widget ui-widget-content ui-helper-clearfix ui-corner-all'></div>"))
    }

    function s(t) {
        var n = "button, .ui-datepicker-prev, .ui-datepicker-next, .ui-datepicker-calendar td a";
        return t.delegate(n, "mouseout", function () {
            e(this).removeClass("ui-state-hover"), this.className.indexOf("ui-datepicker-prev") !== -1 && e(this).removeClass("ui-datepicker-prev-hover"), this.className.indexOf("ui-datepicker-next") !== -1 && e(this).removeClass("ui-datepicker-next-hover")
        }).delegate(n, "mouseover", function () {
            e.datepicker._isDisabledDatepicker(r.inline ? t.parent()[0] : r.input[0]) || (e(this).parents(".ui-datepicker-calendar").find("a").removeClass("ui-state-hover"), e(this).addClass("ui-state-hover"), this.className.indexOf("ui-datepicker-prev") !== -1 && e(this).addClass("ui-datepicker-prev-hover"), this.className.indexOf("ui-datepicker-next") !== -1 && e(this).addClass("ui-datepicker-next-hover"))
        })
    }

    function o(t, n) {
        e.extend(t, n);
        for (var r in n) n[r] == null && (t[r] = n[r]);
        return t
    }
    e.extend(e.ui, {
        datepicker: {
            version: "1.10.4"
        }
    });
    var n = "datepicker",
        r;
    e.extend(i.prototype, {
        markerClassName: "hasDatepicker",
        maxRows: 4,
        _widgetDatepicker: function () {
            return this.dpDiv
        },
        setDefaults: function (e) {
            return o(this._defaults, e || {}), this
        },
        _attachDatepicker: function (t, n) {
            var r, i, s;
            r = t.nodeName.toLowerCase(), i = r === "div" || r === "span", t.id || (this.uuid += 1, t.id = "dp" + this.uuid), s = this._newInst(e(t), i), s.settings = e.extend({}, n || {}), r === "input" ? this._connectDatepicker(t, s) : i && this._inlineDatepicker(t, s)
        },
        _newInst: function (t, n) {
            var r = t[0].id.replace(/([^A-Za-z0-9_\-])/g, "\\\\$1");
            return {
                id: r,
                input: t,
                selectedDay: 0,
                selectedMonth: 0,
                selectedYear: 0,
                drawMonth: 0,
                drawYear: 0,
                inline: n,
                dpDiv: n ? s(e("<div class='" + this._inlineClass + " ui-datepicker ui-widget ui-widget-content ui-helper-clearfix ui-corner-all'></div>")) : this.dpDiv
            }
        },
        _connectDatepicker: function (t, r) {
            var i = e(t);
            r.append = e([]), r.trigger = e([]);
            if (i.hasClass(this.markerClassName)) return;
            this._attachments(i, r), i.addClass(this.markerClassName).keydown(this._doKeyDown).keypress(this._doKeyPress).keyup(this._doKeyUp), this._autoSize(r), e.data(t, n, r), r.settings.disabled && this._disableDatepicker(t)
        },
        _attachments: function (t, n) {
            var r, i, s, o = this._get(n, "appendText"),
                u = this._get(n, "isRTL");
            n.append && n.append.remove(), o && (n.append = e("<span class='" + this._appendClass + "'>" + o + "</span>"), t[u ? "before" : "after"](n.append)), t.unbind("focus", this._showDatepicker), n.trigger && n.trigger.remove(), r = this._get(n, "showOn"), (r === "focus" || r === "both") && t.focus(this._showDatepicker);
            if (r === "button" || r === "both") i = this._get(n, "buttonText"), s = this._get(n, "buttonImage"), n.trigger = e(this._get(n, "buttonImageOnly") ? e("<img/>").addClass(this._triggerClass).attr({
                src: s,
                alt: i,
                title: i
            }) : e("<button type='button'></button>").addClass(this._triggerClass).html(s ? e("<img/>").attr({
                src: s,
                alt: i,
                title: i
            }) : i)), t[u ? "before" : "after"](n.trigger), n.trigger.click(function () {
                return e.datepicker._datepickerShowing && e.datepicker._lastInput === t[0] ? e.datepicker._hideDatepicker() : e.datepicker._datepickerShowing && e.datepicker._lastInput !== t[0] ? (e.datepicker._hideDatepicker(), e.datepicker._showDatepicker(t[0])) : e.datepicker._showDatepicker(t[0]), !1
            })
        },
        _autoSize: function (e) {
            if (this._get(e, "autoSize") && !e.inline) {
                var t, n, r, i, s = new Date(2009, 11, 20),
                    o = this._get(e, "dateFormat");
                o.match(/[DM]/) && (t = function (e) {
                    n = 0, r = 0;
                    for (i = 0; i < e.length; i++) e[i].length > n && (n = e[i].length, r = i);
                    return r
                }, s.setMonth(t(this._get(e, o.match(/MM/) ? "monthNames" : "monthNamesShort"))), s.setDate(t(this._get(e, o.match(/DD/) ? "dayNames" : "dayNamesShort")) + 20 - s.getDay())), e.input.attr("size", this._formatDate(e, s).length)
            }
        },
        _inlineDatepicker: function (t, r) {
            var i = e(t);
            if (i.hasClass(this.markerClassName)) return;
            i.addClass(this.markerClassName).append(r.dpDiv), e.data(t, n, r), this._setDate(r, this._getDefaultDate(r), !0), this._updateDatepicker(r), this._updateAlternate(r), r.settings.disabled && this._disableDatepicker(t), r.dpDiv.css("display", "block")
        },
        _dialogDatepicker: function (t, r, i, s, u) {
            var a, f, l, c, h, p = this._dialogInst;
            return p || (this.uuid += 1, a = "dp" + this.uuid, this._dialogInput = e("<input type='text' id='" + a + "' style='position: absolute; top: -100px; width: 0px;'/>"), this._dialogInput.keydown(this._doKeyDown), e("body").append(this._dialogInput), p = this._dialogInst = this._newInst(this._dialogInput, !1), p.settings = {}, e.data(this._dialogInput[0], n, p)), o(p.settings, s || {}), r = r && r.constructor === Date ? this._formatDate(p, r) : r, this._dialogInput.val(r), this._pos = u ? u.length ? u : [u.pageX, u.pageY] : null, this._pos || (f = document.documentElement.clientWidth, l = document.documentElement.clientHeight, c = document.documentElement.scrollLeft || document.body.scrollLeft, h = document.documentElement.scrollTop || document.body.scrollTop, this._pos = [f / 2 - 100 + c, l / 2 - 150 + h]), this._dialogInput.css("left", this._pos[0] + 20 + "px").css("top", this._pos[1] + "px"), p.settings.onSelect = i, this._inDialog = !0, this.dpDiv.addClass(this._dialogClass), this._showDatepicker(this._dialogInput[0]), e.blockUI && e.blockUI(this.dpDiv), e.data(this._dialogInput[0], n, p), this
        },
        _destroyDatepicker: function (t) {
            var r, i = e(t),
                s = e.data(t, n);
            if (!i.hasClass(this.markerClassName)) return;
            r = t.nodeName.toLowerCase(), e.removeData(t, n), r === "input" ? (s.append.remove(), s.trigger.remove(), i.removeClass(this.markerClassName).unbind("focus", this._showDatepicker).unbind("keydown", this._doKeyDown).unbind("keypress", this._doKeyPress).unbind("keyup", this._doKeyUp)) : (r === "div" || r === "span") && i.removeClass(this.markerClassName).empty()
        },
        _enableDatepicker: function (t) {
            var r, i, s = e(t),
                o = e.data(t, n);
            if (!s.hasClass(this.markerClassName)) return;
            r = t.nodeName.toLowerCase();
            if (r === "input") t.disabled = !1, o.trigger.filter("button").each(function () {
                this.disabled = !1
            }).end().filter("img").css({
                opacity: "1.0",
                cursor: ""
            });
            else if (r === "div" || r === "span") i = s.children("." + this._inlineClass), i.children().removeClass("ui-state-disabled"), i.find("select.ui-datepicker-month, select.ui-datepicker-year").prop("disabled", !1);
            this._disabledInputs = e.map(this._disabledInputs, function (e) {
                return e === t ? null : e
            })
        },
        _disableDatepicker: function (t) {
            var r, i, s = e(t),
                o = e.data(t, n);
            if (!s.hasClass(this.markerClassName)) return;
            r = t.nodeName.toLowerCase();
            if (r === "input") t.disabled = !0, o.trigger.filter("button").each(function () {
                this.disabled = !0
            }).end().filter("img").css({
                opacity: "0.5",
                cursor: "default"
            });
            else if (r === "div" || r === "span") i = s.children("." + this._inlineClass), i.children().addClass("ui-state-disabled"), i.find("select.ui-datepicker-month, select.ui-datepicker-year").prop("disabled", !0);
            this._disabledInputs = e.map(this._disabledInputs, function (e) {
                return e === t ? null : e
            }), this._disabledInputs[this._disabledInputs.length] = t
        },
        _isDisabledDatepicker: function (e) {
            if (!e) return !1;
            for (var t = 0; t < this._disabledInputs.length; t++)
                if (this._disabledInputs[t] === e) return !0;
            return !1
        },
        _getInst: function (t) {
            try {
                return e.data(t, n)
            } catch (r) {
                throw "Missing instance data for this datepicker"
            }
        },
        _optionDatepicker: function (n, r, i) {
            var s, u, a, f, l = this._getInst(n);
            if (arguments.length === 2 && typeof r == "string") return r === "defaults" ? e.extend({}, e.datepicker._defaults) : l ? r === "all" ? e.extend({}, l.settings) : this._get(l, r) : null;
            s = r || {}, typeof r == "string" && (s = {}, s[r] = i), l && (this._curInst === l && this._hideDatepicker(), u = this._getDateDatepicker(n, !0), a = this._getMinMaxDate(l, "min"), f = this._getMinMaxDate(l, "max"), o(l.settings, s), a !== null && s.dateFormat !== t && s.minDate === t && (l.settings.minDate = this._formatDate(l, a)), f !== null && s.dateFormat !== t && s.maxDate === t && (l.settings.maxDate = this._formatDate(l, f)), "disabled" in s && (s.disabled ? this._disableDatepicker(n) : this._enableDatepicker(n)), this._attachments(e(n), l), this._autoSize(l), this._setDate(l, u), this._updateAlternate(l), this._updateDatepicker(l))
        },
        _changeDatepicker: function (e, t, n) {
            this._optionDatepicker(e, t, n)
        },
        _refreshDatepicker: function (e) {
            var t = this._getInst(e);
            t && this._updateDatepicker(t)
        },
        _setDateDatepicker: function (e, t) {
            var n = this._getInst(e);
            n && (this._setDate(n, t), this._updateDatepicker(n), this._updateAlternate(n))
        },
        _getDateDatepicker: function (e, t) {
            var n = this._getInst(e);
            return n && !n.inline && this._setDateFromField(n, t), n ? this._getDate(n) : null
        },
        _doKeyDown: function (t) {
            var n, r, i, s = e.datepicker._getInst(t.target),
                o = !0,
                u = s.dpDiv.is(".ui-datepicker-rtl");
            s._keyEvent = !0;
            if (e.datepicker._datepickerShowing) switch (t.keyCode) {
                case 9:
                    e.datepicker._hideDatepicker(), o = !1;
                    break;
                case 13:
                    return i = e("td." + e.datepicker._dayOverClass + ":not(." + e.datepicker._currentClass + ")", s.dpDiv), i[0] && e.datepicker._selectDay(t.target, s.selectedMonth, s.selectedYear, i[0]), n = e.datepicker._get(s, "onSelect"), n ? (r = e.datepicker._formatDate(s), n.apply(s.input ? s.input[0] : null, [r, s])) : e.datepicker._hideDatepicker(), !1;
                case 27:
                    e.datepicker._hideDatepicker();
                    break;
                case 33:
                    e.datepicker._adjustDate(t.target, t.ctrlKey ? -e.datepicker._get(s, "stepBigMonths") : -e.datepicker._get(s, "stepMonths"), "M");
                    break;
                case 34:
                    e.datepicker._adjustDate(t.target, t.ctrlKey ? +e.datepicker._get(s, "stepBigMonths") : +e.datepicker._get(s, "stepMonths"), "M");
                    break;
                case 35:
                    (t.ctrlKey || t.metaKey) && e.datepicker._clearDate(t.target), o = t.ctrlKey || t.metaKey;
                    break;
                case 36:
                    (t.ctrlKey || t.metaKey) && e.datepicker._gotoToday(t.target), o = t.ctrlKey || t.metaKey;
                    break;
                case 37:
                    (t.ctrlKey || t.metaKey) && e.datepicker._adjustDate(t.target, u ? 1 : -1, "D"), o = t.ctrlKey || t.metaKey, t.originalEvent.altKey && e.datepicker._adjustDate(t.target, t.ctrlKey ? -e.datepicker._get(s, "stepBigMonths") : -e.datepicker._get(s, "stepMonths"), "M");
                    break;
                case 38:
                    (t.ctrlKey || t.metaKey) && e.datepicker._adjustDate(t.target, -7, "D"), o = t.ctrlKey || t.metaKey;
                    break;
                case 39:
                    (t.ctrlKey || t.metaKey) && e.datepicker._adjustDate(t.target, u ? -1 : 1, "D"), o = t.ctrlKey || t.metaKey, t.originalEvent.altKey && e.datepicker._adjustDate(t.target, t.ctrlKey ? +e.datepicker._get(s, "stepBigMonths") : +e.datepicker._get(s, "stepMonths"), "M");
                    break;
                case 40:
                    (t.ctrlKey || t.metaKey) && e.datepicker._adjustDate(t.target, 7, "D"), o = t.ctrlKey || t.metaKey;
                    break;
                default:
                    o = !1
            } else t.keyCode === 36 && t.ctrlKey ? e.datepicker._showDatepicker(this) : o = !1;
            o && (t.preventDefault(), t.stopPropagation())
        },
        _doKeyPress: function (t) {
            var n, r, i = e.datepicker._getInst(t.target);
            if (e.datepicker._get(i, "constrainInput")) return n = e.datepicker._possibleChars(e.datepicker._get(i, "dateFormat")), r = String.fromCharCode(t.charCode == null ? t.keyCode : t.charCode), t.ctrlKey || t.metaKey || r < " " || !n || n.indexOf(r) > -1
        },
        _doKeyUp: function (t) {
            var n, r = e.datepicker._getInst(t.target);
            if (r.input.val() !== r.lastVal) try {
                n = e.datepicker.parseDate(e.datepicker._get(r, "dateFormat"), r.input ? r.input.val() : null, e.datepicker._getFormatConfig(r)), n && (e.datepicker._setDateFromField(r), e.datepicker._updateAlternate(r), e.datepicker._updateDatepicker(r))
            } catch (i) { }
            return !0
        },
        _showDatepicker: function (t) {
            t = t.target || t, t.nodeName.toLowerCase() !== "input" && (t = e("input", t.parentNode)[0]);
            if (e.datepicker._isDisabledDatepicker(t) || e.datepicker._lastInput === t) return;
            var n, r, i, s, u, a, f;
            n = e.datepicker._getInst(t), e.datepicker._curInst && e.datepicker._curInst !== n && (e.datepicker._curInst.dpDiv.stop(!0, !0), n && e.datepicker._datepickerShowing && e.datepicker._hideDatepicker(e.datepicker._curInst.input[0])), r = e.datepicker._get(n, "beforeShow"), i = r ? r.apply(t, [t, n]) : {};
            if (i === !1) return;
            o(n.settings, i), n.lastVal = null, e.datepicker._lastInput = t, e.datepicker._setDateFromField(n), e.datepicker._inDialog && (t.value = ""), e.datepicker._pos || (e.datepicker._pos = e.datepicker._findPos(t), e.datepicker._pos[1] += t.offsetHeight), s = !1, e(t).parents().each(function () {
                return s |= e(this).css("position") === "fixed", !s
            }), u = {
                left: e.datepicker._pos[0],
                top: e.datepicker._pos[1]
            }, e.datepicker._pos = null, n.dpDiv.empty(), n.dpDiv.css({
                position: "absolute",
                display: "block",
                top: "-1000px"
            }), e.datepicker._updateDatepicker(n), u = e.datepicker._checkOffset(n, u, s), n.dpDiv.css({
                position: e.datepicker._inDialog && e.blockUI ? "static" : s ? "fixed" : "absolute",
                display: "none",
                left: u.left + "px",
                top: u.top + "px"
            }), n.inline || (a = e.datepicker._get(n, "showAnim"), f = e.datepicker._get(n, "duration"), n.dpDiv.zIndex(e(t).zIndex() + 99999999), e.datepicker._datepickerShowing = !0, e.effects && e.effects.effect[a] ? n.dpDiv.show(a, e.datepicker._get(n, "showOptions"), f) : n.dpDiv[a || "show"](a ? f : null), e.datepicker._shouldFocusInput(n) && n.input.focus(), e.datepicker._curInst = n)
        },
        _updateDatepicker: function (t) {
            this.maxRows = 4, r = t, t.dpDiv.empty().append(this._generateHTML(t)), this._attachHandlers(t), t.dpDiv.find("." + this._dayOverClass + " a").mouseover();
            var n, i = this._getNumberOfMonths(t),
                s = i[1],
                o = 17;
            t.dpDiv.removeClass("ui-datepicker-multi-2 ui-datepicker-multi-3 ui-datepicker-multi-4").width(""), s > 1 && t.dpDiv.addClass("ui-datepicker-multi-" + s).css("width", o * s + "em"), t.dpDiv[(i[0] !== 1 || i[1] !== 1 ? "add" : "remove") + "Class"]("ui-datepicker-multi"), t.dpDiv[(this._get(t, "isRTL") ? "add" : "remove") + "Class"]("ui-datepicker-rtl"), t === e.datepicker._curInst && e.datepicker._datepickerShowing && e.datepicker._shouldFocusInput(t) && t.input.focus(), t.yearshtml && (n = t.yearshtml, setTimeout(function () {
                n === t.yearshtml && t.yearshtml && t.dpDiv.find("select.ui-datepicker-year:first").replaceWith(t.yearshtml), n = t.yearshtml = null
            }, 0));
            var u = this._get(t, "showFareTrends") ? this._get(t, "showFareTrends") : !1;
            u && showTrendsAgain()
        },
        _shouldFocusInput: function (e) {
            return e.input && e.input.is(":visible") && !e.input.is(":disabled") && !e.input.is(":focus")
        },
        _checkOffset: function (t, n, r) {
            var i = t.dpDiv.outerWidth(),
                s = t.dpDiv.outerHeight(),
                o = t.input ? t.input.outerWidth() : 0,
                u = t.input ? t.input.outerHeight() : 0,
                a = document.documentElement.clientWidth + (r ? 0 : e(document).scrollLeft()),
                f = document.documentElement.clientHeight + (r ? 0 : e(document).scrollTop());
            return n.left -= this._get(t, "isRTL") ? i - o : 0, n.left -= r && n.left === t.input.offset().left ? e(document).scrollLeft() : 0, n.top -= r && n.top === t.input.offset().top + u ? e(document).scrollTop() : 0, n.left -= Math.min(n.left, n.left + i > a && a > i ? Math.abs(n.left + i - a) : 0), n.top -= Math.min(n.top, n.top + s > f && f > s ? Math.abs(s + u) : 0), n
        },
        _findPos: function (t) {
            var n = t,
                r, i = this._getInst(t),
                s = this._get(i, "isRTL");
            while (t && (t.type === "hidden" || t.nodeType !== 1 || e.expr.filters.hidden(t))) t = t[s ? "previousSibling" : "nextElementSibling"];
            return t == null && (t = n[s ? "previousSibling" : "nextElementSibling"]), r = e(t).offset(), [r.left, r.top + 15]
        },
        _hideDatepicker: function (t) {
            var r, i, s, o, u = this._curInst;
            if (!u || t && u !== e.data(t, n)) return;
            this._datepickerShowing && (r = this._get(u, "showAnim"), i = this._get(u, "duration"), s = function () {
                e.datepicker._tidyDialog(u)
            }, e.effects && (e.effects.effect[r] || e.effects[r]) ? u.dpDiv.hide(r, e.datepicker._get(u, "showOptions"), i, s) : u.dpDiv[r === "slideDown" ? "slideUp" : r === "fadeIn" ? "fadeOut" : "hide"](r ? i : null, s), r || s(), this._datepickerShowing = !1, o = this._get(u, "onClose"), o && o.apply(u.input ? u.input[0] : null, [u.input ? u.input.val() : "", u]), this._lastInput = null, this._inDialog && (this._dialogInput.css({
                position: "absolute",
                left: "0",
                top: "-100px"
            }), e.blockUI && (e.unblockUI(), e("body").append(this.dpDiv))), this._inDialog = !1)
        },
        _tidyDialog: function (e) {
            e.dpDiv.removeClass(this._dialogClass).unbind(".ui-datepicker-calendar")
        },
        _checkExternalClick: function (t) {
            if (!e.datepicker._curInst) return;
            var n = e(t.target),
                r = e.datepicker._getInst(n[0]);
            (n[0].id !== e.datepicker._mainDivId && n.parents("#" + e.datepicker._mainDivId).length === 0 && !n.hasClass(e.datepicker.markerClassName) && !n.closest("." + e.datepicker._triggerClass).length && e.datepicker._datepickerShowing && (!e.datepicker._inDialog || !e.blockUI) || n.hasClass(e.datepicker.markerClassName) && e.datepicker._curInst !== r) && e.datepicker._hideDatepicker()
        },
        _adjustDate: function (t, n, r) {
            var i = e(t),
                s = this._getInst(i[0]);
            if (this._isDisabledDatepicker(i[0])) return;
            this._adjustInstDate(s, n + (r === "M" ? this._get(s, "showCurrentAtPos") : 0), r), this._updateDatepicker(s)
        },
        _gotoToday: function (t) {
            var n, r = e(t),
                i = this._getInst(r[0]);
            this._get(i, "gotoCurrent") && i.currentDay ? (i.selectedDay = i.currentDay, i.drawMonth = i.selectedMonth = i.currentMonth, i.drawYear = i.selectedYear = i.currentYear) : (n = new Date, i.selectedDay = n.getDate(), i.drawMonth = i.selectedMonth = n.getMonth(), i.drawYear = i.selectedYear = n.getFullYear()), this._notifyChange(i), this._adjustDate(r)
        },
        _selectMonthYear: function (t, n, r) {
            var i = e(t),
                s = this._getInst(i[0]);
            s["selected" + (r === "M" ? "Month" : "Year")] = s["draw" + (r === "M" ? "Month" : "Year")] = parseInt(n.options[n.selectedIndex].value, 10), this._notifyChange(s), this._adjustDate(i)
        },
        _selectDay: function (t, n, r, i) {
            var s, o = e(t);
            if (e(i).hasClass(this._unselectableClass) || this._isDisabledDatepicker(o[0])) return;
            s = this._getInst(o[0]), s.selectedDay = s.currentDay = e("a", i).html(), s.selectedMonth = s.currentMonth = n, s.selectedYear = s.currentYear = r, this._selectDate(t, this._formatDate(s, s.currentDay, s.currentMonth, s.currentYear))
        },
        _clearDate: function (t) {
            var n = e(t);
            this._selectDate(n, "")
        },
        _selectDate: function (t, n) {
            var r, i = e(t),
                s = this._getInst(i[0]);
            n = n != null ? n : this._formatDate(s), s.input && s.input.val(n), this._updateAlternate(s), r = this._get(s, "onSelect"), r ? r.apply(s.input ? s.input[0] : null, [n, s]) : s.input && s.input.trigger("change"), s.inline ? this._updateDatepicker(s) : (this._hideDatepicker(), this._lastInput = s.input[0], typeof s.input[0] != "object" && s.input.focus(), this._lastInput = null)
        },
        _updateAlternate: function (t) {
            var n, r, i, s = this._get(t, "altField");
            s && (n = this._get(t, "altFormat") || this._get(t, "dateFormat"), r = this._getDate(t), i = this.formatDate(n, r, this._getFormatConfig(t)), e(s).each(function () {
                e(this).val(i)
            }))
        },
        noWeekends: function (e) {
            var t = e.getDay();
            return [t > 0 && t < 6, ""]
        },
        iso8601Week: function (e) {
            var t, n = new Date(e.getTime());
            return n.setDate(n.getDate() + 4 - (n.getDay() || 7)), t = n.getTime(), n.setMonth(0), n.setDate(1), Math.floor(Math.round((t - n) / 864e5) / 7) + 1
        },
        parseDate: function (t, n, r) {
            if (t == null || n == null) throw "Invalid arguments";
            n = typeof n == "object" ? n.toString() : n + "";
            if (n === "") return null;
            var i, s, o, u = 0,
                a = (r ? r.shortYearCutoff : null) || this._defaults.shortYearCutoff,
                f = typeof a != "string" ? a : (new Date).getFullYear() % 100 + parseInt(a, 10),
                l = (r ? r.dayNamesShort : null) || this._defaults.dayNamesShort,
                c = (r ? r.dayNames : null) || this._defaults.dayNames,
                h = (r ? r.monthNamesShort : null) || this._defaults.monthNamesShort,
                p = (r ? r.monthNames : null) || this._defaults.monthNames,
                d = -1,
                v = -1,
                m = -1,
                g = -1,
                y = !1,
                b, w = function (e) {
                    var n = i + 1 < t.length && t.charAt(i + 1) === e;
                    return n && i++, n
                },
                E = function (e) {
                    var t = w(e),
                        r = e === "@" ? 14 : e === "!" ? 20 : e === "y" && t ? 4 : e === "o" ? 3 : 2,
                        i = new RegExp("^\\d{1," + r + "}"),
                        s = n.substring(u).match(i);
                    if (!s) throw "Missing number at position " + u;
                    return u += s[0].length, parseInt(s[0], 10)
                },
                S = function (t, r, i) {
                    var s = -1,
                        o = e.map(w(t) ? i : r, function (e, t) {
                            return [
                                [t, e]
                            ]
                        }).sort(function (e, t) {
                            return -(e[1].length - t[1].length)
                        });
                    e.each(o, function (e, t) {
                        var r = t[1];
                        if (n.substr(u, r.length).toLowerCase() === r.toLowerCase()) return s = t[0], u += r.length, !1
                    });
                    if (s !== -1) return s + 1;
                    throw "Unknown name at position " + u
                },
                x = function () {
                    if (n.charAt(u) !== t.charAt(i)) throw "Unexpected literal at position " + u;
                    u++
                };
            for (i = 0; i < t.length; i++)
                if (y) t.charAt(i) === "'" && !w("'") ? y = !1 : x();
                else switch (t.charAt(i)) {
                    case "d":
                        m = E("d");
                        break;
                    case "D":
                        S("D", l, c);
                        break;
                    case "o":
                        g = E("o");
                        break;
                    case "m":
                        v = E("m");
                        break;
                    case "M":
                        v = S("M", h, p);
                        break;
                    case "y":
                        d = E("y");
                        break;
                    case "@":
                        b = new Date(E("@")), d = b.getFullYear(), v = b.getMonth() + 1, m = b.getDate();
                        break;
                    case "!":
                        b = new Date((E("!") - this._ticksTo1970) / 1e4), d = b.getFullYear(), v = b.getMonth() + 1, m = b.getDate();
                        break;
                    case "'":
                        w("'") ? x() : y = !0;
                        break;
                    default:
                        x()
                }
            if (u < n.length) {
                o = n.substr(u);
                if (!/^\s+/.test(o)) throw "Extra/unparsed characters found in date: " + o
            }
            d === -1 ? d = (new Date).getFullYear() : d < 100 && (d += (new Date).getFullYear() - (new Date).getFullYear() % 100 + (d <= f ? 0 : -100));
            if (g > -1) {
                v = 1, m = g;
                do {
                    s = this._getDaysInMonth(d, v - 1);
                    if (m <= s) break;
                    v++, m -= s
                } while (!0)
            }
            b = this._daylightSavingAdjust(new Date(d, v - 1, m));
            if (b.getFullYear() !== d || b.getMonth() + 1 !== v || b.getDate() !== m) throw "Invalid date";
            return b
        },
        ATOM: "yy-mm-dd",
        COOKIE: "D, dd M yy",
        ISO_8601: "yy-mm-dd",
        RFC_822: "D, d M y",
        RFC_850: "DD, dd-M-y",
        RFC_1036: "D, d M y",
        RFC_1123: "D, d M yy",
        RFC_2822: "D, d M yy",
        RSS: "D, d M y",
        TICKS: "!",
        TIMESTAMP: "@",
        W3C: "yy-mm-dd",
        _ticksTo1970: (718685 + Math.floor(492.5) - Math.floor(19.7) + Math.floor(4.925)) * 24 * 60 * 60 * 1e7,
        formatDate: function (e, t, n) {
            if (!t) return "";
            var r, i = (n ? n.dayNamesShort : null) || this._defaults.dayNamesShort,
                s = (n ? n.dayNames : null) || this._defaults.dayNames,
                o = (n ? n.monthNamesShort : null) || this._defaults.monthNamesShort,
                u = (n ? n.monthNames : null) || this._defaults.monthNames,
                a = function (t) {
                    var n = r + 1 < e.length && e.charAt(r + 1) === t;
                    return n && r++, n
                },
                f = function (e, t, n) {
                    var r = "" + t;
                    if (a(e))
                        while (r.length < n) r = "0" + r;
                    return r
                },
                l = function (e, t, n, r) {
                    return a(e) ? r[t] : n[t]
                },
                c = "",
                h = !1;
            if (t)
                for (r = 0; r < e.length; r++)
                    if (h) e.charAt(r) === "'" && !a("'") ? h = !1 : c += e.charAt(r);
                    else switch (e.charAt(r)) {
                        case "d":
                            c += f("d", t.getDate(), 2);
                            break;
                        case "D":
                            c += l("D", t.getDay(), i, s);
                            break;
                        case "o":
                            c += f("o", Math.round(((new Date(t.getFullYear(), t.getMonth(), t.getDate())).getTime() - (new Date(t.getFullYear(), 0, 0)).getTime()) / 864e5), 3);
                            break;
                        case "m":
                            c += f("m", t.getMonth() + 1, 2);
                            break;
                        case "M":
                            c += l("M", t.getMonth(), o, u);
                            break;
                        case "y":
                            c += a("y") ? t.getFullYear() : (t.getYear() % 100 < 10 ? "0" : "") + t.getYear() % 100;
                            break;
                        case "@":
                            c += t.getTime();
                            break;
                        case "!":
                            c += t.getTime() * 1e4 + this._ticksTo1970;
                            break;
                        case "'":
                            a("'") ? c += "'" : h = !0;
                            break;
                        default:
                            c += e.charAt(r)
                    }
            return c
        },
        _possibleChars: function (e) {
            var t, n = "",
                r = !1,
                i = function (n) {
                    var r = t + 1 < e.length && e.charAt(t + 1) === n;
                    return r && t++, r
                };
            for (t = 0; t < e.length; t++)
                if (r) e.charAt(t) === "'" && !i("'") ? r = !1 : n += e.charAt(t);
                else switch (e.charAt(t)) {
                    case "d":
                    case "m":
                    case "y":
                    case "@":
                        n += "0123456789";
                        break;
                    case "D":
                    case "M":
                        return null;
                    case "'":
                        i("'") ? n += "'" : r = !0;
                        break;
                    default:
                        n += e.charAt(t)
                }
            return n
        },
        _get: function (e, n) {
            return e.settings[n] !== t ? e.settings[n] : this._defaults[n]
        },
        _setDateFromField: function (e, t) {
            if (e.input.val() === e.lastVal) return;
            var n = this._get(e, "dateFormat"),
                r = e.lastVal = e.input ? e.input.val() : null,
                i = this._getDefaultDate(e),
                s = i,
                o = this._getFormatConfig(e);
            try {
                s = this.parseDate(n, r, o) || i
            } catch (u) {
                r = t ? "" : r
            }
            e.selectedDay = s.getDate(), e.drawMonth = e.selectedMonth = s.getMonth(), e.drawYear = e.selectedYear = s.getFullYear(), e.currentDay = r ? s.getDate() : 0, e.currentMonth = r ? s.getMonth() : 0, e.currentYear = r ? s.getFullYear() : 0, this._adjustInstDate(e)
        },
        _getDefaultDate: function (e) {
            return this._restrictMinMax(e, this._determineDate(e, this._get(e, "defaultDate"), new Date))
        },
        _determineDate: function (t, n, r) {
            var i = function (e) {
                var t = new Date;
                return t.setDate(t.getDate() + e), t
            },
                s = function (n) {
                    try {
                        return e.datepicker.parseDate(e.datepicker._get(t, "dateFormat"), n, e.datepicker._getFormatConfig(t))
                    } catch (r) { }
                    var i = (n.toLowerCase().match(/^c/) ? e.datepicker._getDate(t) : null) || new Date,
                        s = i.getFullYear(),
                        o = i.getMonth(),
                        u = i.getDate(),
                        a = /([+\-]?[0-9]+)\s*(d|D|w|W|m|M|y|Y)?/g,
                        f = a.exec(n);
                    while (f) {
                        switch (f[2] || "d") {
                            case "d":
                            case "D":
                                u += parseInt(f[1], 10);
                                break;
                            case "w":
                            case "W":
                                u += parseInt(f[1], 10) * 7;
                                break;
                            case "m":
                            case "M":
                                o += parseInt(f[1], 10), u = Math.min(u, e.datepicker._getDaysInMonth(s, o));
                                break;
                            case "y":
                            case "Y":
                                s += parseInt(f[1], 10), u = Math.min(u, e.datepicker._getDaysInMonth(s, o))
                        }
                        f = a.exec(n)
                    }
                    return new Date(s, o, u)
                },
                o = n == null || n === "" ? r : typeof n == "string" ? s(n) : typeof n == "number" ? isNaN(n) ? r : i(n) : new Date(n.getTime());
            return o = o && o.toString() === "Invalid Date" ? r : o, o && (o.setHours(0), o.setMinutes(0), o.setSeconds(0), o.setMilliseconds(0)), this._daylightSavingAdjust(o)
        },
        _daylightSavingAdjust: function (e) {
            return e ? (e.setHours(e.getHours() > 12 ? e.getHours() + 2 : 0), e) : null
        },
        _setDate: function (e, t, n) {
            var r = !t,
                i = e.selectedMonth,
                s = e.selectedYear,
                o = this._restrictMinMax(e, this._determineDate(e, t, new Date));
            e.selectedDay = e.currentDay = o.getDate(), e.drawMonth = e.selectedMonth = e.currentMonth = o.getMonth(), e.drawYear = e.selectedYear = e.currentYear = o.getFullYear(), (i !== e.selectedMonth || s !== e.selectedYear) && !n && this._notifyChange(e), this._adjustInstDate(e), e.input && e.input.val(r ? "" : this._formatDate(e))
        },
        _getDate: function (e) {
            var t = !e.currentYear || e.input && e.input.val() === "" ? null : this._daylightSavingAdjust(new Date(e.currentYear, e.currentMonth, e.currentDay));
            return t
        },
        _attachHandlers: function (t) {
            var n = this._get(t, "stepMonths"),
                r = "#" + t.id.replace(/\\\\/g, "\\");
            t.dpDiv.find("[data-handler]").map(function () {
                var t = {
                    prev: function () {
                        e.datepicker._adjustDate(r, -n, "M")
                    },
                    next: function () {
                        e.datepicker._adjustDate(r, +n, "M")
                    },
                    hide: function () {
                        e.datepicker._hideDatepicker()
                    },
                    today: function () {
                        e.datepicker._gotoToday(r)
                    },
                    selectDay: function () {
                        return e.datepicker._selectDay(r, +this.getAttribute("data-month"), +this.getAttribute("data-year"), this), !1
                    },
                    selectMonth: function () {
                        return e.datepicker._selectMonthYear(r, this, "M"), !1
                    },
                    selectYear: function () {
                        return e.datepicker._selectMonthYear(r, this, "Y"), !1
                    }
                };
                e(this).bind(this.getAttribute("data-event"), t[this.getAttribute("data-handler")])
            })
        },
        _generateHTML: function (e) {
            var t, n, r, i, s, o, u, a, f, l, c, h, p, d, v, m, g, y, b, w, E, S, x, T, N, C, k, L, A, O, M, _, D, P, H, B, j, F, I, q = new Date,
                R = this._daylightSavingAdjust(new Date(q.getFullYear(), q.getMonth(), q.getDate())),
                U = this._get(e, "isRTL"),
                z = this._get(e, "showButtonPanel"),
                W = this._get(e, "showCalenderName"),
                X = this._get(e, "showCalenderHotelPopupName"),
                V = this._get(e, "showOnewaySearchText"),
                $ = this._get(e, "hideIfNoPrevNext"),
                J = this._get(e, "navigationAsDateFormat"),
                K = this._getNumberOfMonths(e),
                Q = this._get(e, "showCurrentAtPos"),
                G = this._get(e, "stepMonths"),
                Y = K[0] !== 1 || K[1] !== 1,
                Z = this._daylightSavingAdjust(e.currentDay ? new Date(e.currentYear, e.currentMonth, e.currentDay) : new Date(9999, 9, 9)),
                et = this._getMinMaxDate(e, "min"),
                tt = this._getMinMaxDate(e, "max"),
                nt = e.drawMonth - Q,
                rt = e.drawYear,
                it = this._get(e, "showFareTrends") ? this._get(e, "showFareTrends") : !1;
            nt < 0 && (nt += 12, rt--);
            if (tt) {
                t = this._daylightSavingAdjust(new Date(tt.getFullYear(), tt.getMonth() - K[0] * K[1] + 1, tt.getDate())), t = et && t < et ? et : t;
                while (this._daylightSavingAdjust(new Date(rt, nt, 1)) > t) nt--, nt < 0 && (nt = 11, rt--)
            }
            e.drawMonth = nt, e.drawYear = rt, n = this._get(e, "prevText"), n = J ? this.formatDate(n, this._daylightSavingAdjust(new Date(rt, nt - G, 1)), this._getFormatConfig(e)) : n, r = this._canAdjustMonth(e, -1, rt, nt) ? "<a class='ui-datepicker-prev ui-corner-all' data-handler='prev' data-event='click' title='" + n + "'><span class='ui-icon ui-icon-circle-triangle-" + (U ? "e" : "w") + "'>" + n + "</span></a>" : $ ? "" : "<a class='ui-datepicker-prev ui-corner-all ui-state-disabled' title='" + n + "'><span class='ui-icon ui-icon-circle-triangle-" + (U ? "e" : "w") + "'>" + n + "</span></a>", i = this._get(e, "nextText"), i = J ? this.formatDate(i, this._daylightSavingAdjust(new Date(rt, nt + G, 1)), this._getFormatConfig(e)) : i, s = this._canAdjustMonth(e, 1, rt, nt) ? "<a class='ui-datepicker-next ui-corner-all' data-handler='next' data-event='click' title='" + i + "'><span class='ui-icon ui-icon-circle-triangle-" + (U ? "w" : "e") + "'>" + i + "</span></a>" : $ ? "" : "<a class='ui-datepicker-next ui-corner-all ui-state-disabled' title='" + i + "'><span class='ui-icon ui-icon-circle-triangle-" + (U ? "w" : "e") + "'>" + i + "</span></a>", o = this._get(e, "currentText"), u = this._get(e, "gotoCurrent") && e.currentDay ? Z : R, o = J ? this.formatDate(o, u, this._getFormatConfig(e)) : o, a = e.inline ? "" : "<button type='button' class='ui-datepicker-close ui-state-default ui-priority-primary ui-corner-all' data-handler='hide' data-event='click'>" + this._get(e, "closeText") + "<span class='ui-datepicker-close'><span></button>", f = z ? "<div class='ui-datepicker-buttonpane ui-widget-content'>" + (U ? a : "") + (this._isInRange(e, u) ? "<button type='button' class='ui-datepicker-current ui-state-default ui-priority-secondary ui-corner-all' data-handler='today' data-event='click'>" + o + "</button>" : "") + (U ? "" : a) + "</div>" : "", l = parseInt(this._get(e, "firstDay"), 10), l = isNaN(l) ? 0 : l, c = this._get(e, "showWeek"), h = this._get(e, "dayNames"), p = this._get(e, "dayNamesMin"), d = this._get(e, "monthNames"), v = this._get(e, "monthNamesShort"), m = this._get(e, "beforeShowDay"), g = this._get(e, "showOtherMonths"), y = this._get(e, "selectOtherMonths"), b = this._getDefaultDate(e), w = "", E;
            for (S = 0; S < K[0]; S++) {
                x = "", this.maxRows = 4;
                for (T = 0; T < K[1]; T++) {
                    N = this._daylightSavingAdjust(new Date(rt, nt, e.selectedDay)), C = " ui-corner-all", k = "";
                    if (Y) {
                        k += "<div class='ui-datepicker-group";
                        if (K[1] > 1) switch (T) {
                            case 0:
                                k += " ui-datepicker-group-first", C = " ui-corner-" + (U ? "right" : "left");
                                break;
                            case K[1] - 1:
                                k += " ui-datepicker-group-last", C = " ui-corner-" + (U ? "left" : "right");
                                break;
                            default:
                                k += " ui-datepicker-group-middle", C = ""
                        }
                        k += "'>"
                    }
                    k += "<div class='ui-datepicker-header ui-widget-header ui-helper-clearfix" + C + "'>" + (/all|left/.test(C) && S === 0 ? U ? s : r : "") + (/all|right/.test(C) && S === 0 ? U ? r : s : "") + this._generateMonthYearHeader(e, nt, rt, et, tt, S > 0 || T > 0, d, v) + "</div><table class='ui-datepicker-calendar'><thead>" + "<tr>", L = c ? "<th class='ui-datepicker-week-col'>" + this._get(e, "weekHeader") + "</th>" : "";
                    for (E = 0; E < 7; E++) A = (E + l) % 7, L += "<th" + ((E + l + 6) % 7 >= 5 ? " class='ui-datepicker-week-end'" : "") + ">" + "<span title='" + h[A] + "'>" + p[A] + "</span></th>";
                    k += L + "</tr></thead><tbody>", O = this._getDaysInMonth(rt, nt), rt === e.selectedYear && nt === e.selectedMonth && (e.selectedDay = Math.min(e.selectedDay, O)), M = (this._getFirstDayOfMonth(rt, nt) - l + 7) % 7, _ = Math.ceil((M + O) / 7), D = Y ? this.maxRows > _ ? this.maxRows : _ : _, this.maxRows = D, P = this._daylightSavingAdjust(new Date(rt, nt, 1 - M));
                    for (H = 0; H < D; H++) {
                        k += "<tr>", B = c ? "<td class='ui-datepicker-week-col'>" + this._get(e, "calculateWeek")(P) + "</td>" : "";
                        for (E = 0; E < 7; E++) j = m ? m.apply(e.input ? e.input[0] : null, [P]) : [!0, ""], F = P.getMonth() !== nt, I = F && !y || !j[0] || et && P < et || tt && P > tt, B += "<td class='" + ((E + l + 6) % 7 >= 5 ? " ui-datepicker-week-end" : "") + (F ? " ui-datepicker-other-month" : "") + (P.getTime() === N.getTime() && nt === e.selectedMonth && e._keyEvent || b.getTime() === P.getTime() && b.getTime() === N.getTime() ? " " + this._dayOverClass : "") + (I ? " " + this._unselectableClass + " ui-state-disabled" : "") + (F && !g ? "" : " " + j[1] + (P.getTime() === Z.getTime() ? " " + this._currentClass : "") + (P.getTime() === R.getTime() ? " ui-datepicker-today" : "")) + "'" + ((!F || g) && j[2] ? " title='" + j[2].replace(/'/g, "&#39;") + "'" : "") + (I ? "" : " data-handler='selectDay' data-event='click' data-month='" + P.getMonth() + "' data-year='" + P.getFullYear() + "'" + "' fare-date='" + P.getDate() + "-" + P.getMonth() + "-" + P.getFullYear() + "'") + ">" + (F && !g ? "&#xa0;" : I ? "<span class='ui-state-default'>" + P.getDate() + "</span>" : "<a class='ui-state-default" + (P.getTime() === R.getTime() ? " ui-state-highlight" : "") + (P.getTime() === Z.getTime() ? " ui-state-active" : "") + (F ? " ui-priority-secondary" : "") + "' href='#'>" + P.getDate() + "</a>") + "</td>", P.setDate(P.getDate() + 1), P = this._daylightSavingAdjust(P);
                        k += B + "</tr>"
                    }
                    nt++, nt > 11 && (nt = 0, rt++), k += "</tbody></table>" + (Y ? "</div>" + (K[0] > 0 && T === K[1] - 1 ? "<div class='ui-datepicker-row-break'></div>" : "") : ""), x += k
                }
                W == 1 && (e.id == "start_date0" || e.id == "start_date1" || e.id == "start_date2" || e.id == "start_date3" || e.id == "start_date4") && (w += '<div class="calender_heading ui-datepicker-title clearfix"><span class="pull-left">&nbsp;</span>', w += "</div>"), W == 1 && e.id == "start_date" && (w += '<div class="calender_heading ui-datepicker-title clearfix"><span class="pull-left">' + CAL_DEPARTURE_TEXT + "</span>", w += "</div>"), W == 1 && e.id == "return_date" && (w += '<div class="calender_heading ui-datepicker-title clearfix"><span class="pull-left">' + CAL_RETURN_TEXT + "</span>", V && (w += '<span class="oneWaySearch pull-left" onclick="change_trip_by_calendar();">' + CAL_ONEWAY_TEXT + "</span>"), w += "</div>"), X == 1 && e.id == "start_date_popup" && (w += '<div class="calender_heading ui-datepicker-title clearfix"><span class="pull-left">' + CAL_DEPARTURE_TEXT + "</span>", w += "</div>"), X == 1 && e.id == "return_date_popup" && (w += '<div class="calender_heading ui-datepicker-title clearfix"><span class="pull-left">' + CAL_RETURN_TEXT + "</span>", V && (w += '<span class="oneWaySearch pull-left" onclick="change_trip_by_calendar();">' + CAL_ONEWAY_TEXT + "</span>"), w += "</div>"), w += x
            }
            return it ? e.id == "start_date" ? w += "<p class='clearfix dp_bottom_info'><span class='hld_lgnd_icn pull-left'></span> <span class='denote_hldys pull-left'>" + CAL_HOLIDAY_TEXT + "</span>   <span class='pull-right showfare_wrapper'><label class='pull-left'><input id='oneway_faretrend' type='checkbox'  onclick='colorDatepicker(response, true);' name='show_fare' class='pull-left' /><span class='show_fare_caption'>Show Fare Trends</span></label><span class='fare_legends pull-left'>Low</span><span class='low_fare_legend pull-left'></span><span class='midH_fare_legend pull-left'></span><span class='high_fare_legend pull-left'></span><span class='fare_legends pull-left'>High</span></span></p>" : e.id == "return_date" ? w += "<p class='clearfix dp_bottom_info'><span class='hld_lgnd_icn pull-left'></span> <span class='denote_hldys pull-left'>" + CAL_HOLIDAY_TEXT + "</span>   <span class='pull-right showfare_wrapper'><label class='pull-left'><input id='roundtrip_faretrend' type='checkbox'  onclick='colorDatepicker(response, true);' name='show_fare' class='pull-left' /><span class='show_fare_caption'>Show Fare Trends</span></label><span class='fare_legends pull-left'>Low</span><span class='low_fare_legend pull-left'></span><span class='midH_fare_legend pull-left'></span><span class='high_fare_legend pull-left'></span><span class='fare_legends pull-left'>High</span></span></p>" : w += "<p class='clearfix dp_bottom_info'><span class='hld_lgnd_icn pull-left'></span> <span class='denote_hldys pull-left'>" + CAL_HOLIDAY_TEXT + "</span>   <span class='pull-right showfare_wrapper'><label class='pull-left'/></span></p>" : w += "<p class='clearfix dp_bottom_info'><span class='hld_lgnd_icn pull-left'></span> <span class='denote_hldys pull-left'>" + CAL_HOLIDAY_TEXT + "</span></p>", w += f, e._keyEvent = !1, w
        },
        _generateMonthYearHeader: function (e, t, n, r, i, s, o, u) {
            var a, f, l, c, h, p, d, v, m = this._get(e, "changeMonth"),
                g = this._get(e, "changeYear"),
                y = this._get(e, "showMonthAfterYear"),
                b = "<div class='ui-datepicker-title'>",
                w = "";
            if (s || !m) w += "<span class='ui-datepicker-month'>" + o[t] + "</span>";
            else {
                a = r && r.getFullYear() === n, f = i && i.getFullYear() === n, w += "<select class='ui-datepicker-month' data-handler='selectMonth' data-event='change'>";
                for (l = 0; l < 12; l++) (!a || l >= r.getMonth()) && (!f || l <= i.getMonth()) && (w += "<option value='" + l + "'" + (l === t ? " selected='selected'" : "") + ">" + u[l] + "</option>");
                w += "</select>"
            }
            y || (b += w + (s || !m || !g ? "&#xa0;" : ""));
            if (!e.yearshtml) {
                e.yearshtml = "";
                if (s || !g) b += "<span class='ui-datepicker-year'>" + n + "</span>";
                else {
                    c = this._get(e, "yearRange").split(":"), h = (new Date).getFullYear(), p = function (e) {
                        var t = e.match(/c[+\-].*/) ? n + parseInt(e.substring(1), 10) : e.match(/[+\-].*/) ? h + parseInt(e, 10) : parseInt(e, 10);
                        return isNaN(t) ? h : t
                    }, d = p(c[0]), v = Math.max(d, p(c[1] || "")), d = r ? Math.max(d, r.getFullYear()) : d, v = i ? Math.min(v, i.getFullYear()) : v, e.yearshtml += "<select class='ui-datepicker-year' data-handler='selectYear' data-event='change'>";
                    for (; d <= v; d++) e.yearshtml += "<option value='" + d + "'" + (d === n ? " selected='selected'" : "") + ">" + d + "</option>";
                    e.yearshtml += "</select>", b += e.yearshtml, e.yearshtml = null
                }
            }
            return b += this._get(e, "yearSuffix"), y && (b += (s || !m || !g ? "&#xa0;" : "") + w), b += "</div>", b
        },
        _adjustInstDate: function (e, t, n) {
            var r = e.drawYear + (n === "Y" ? t : 0),
                i = e.drawMonth + (n === "M" ? t : 0),
                s = Math.min(e.selectedDay, this._getDaysInMonth(r, i)) + (n === "D" ? t : 0),
                o = this._restrictMinMax(e, this._daylightSavingAdjust(new Date(r, i, s)));
            e.selectedDay = o.getDate(), e.drawMonth = e.selectedMonth = o.getMonth(), e.drawYear = e.selectedYear = o.getFullYear(), (n === "M" || n === "Y") && this._notifyChange(e)
        },
        _restrictMinMax: function (e, t) {
            var n = this._getMinMaxDate(e, "min"),
                r = this._getMinMaxDate(e, "max"),
                i = n && t < n ? n : t;
            return r && i > r ? r : i
        },
        _notifyChange: function (e) {
            var t = this._get(e, "onChangeMonthYear");
            t && t.apply(e.input ? e.input[0] : null, [e.selectedYear, e.selectedMonth + 1, e])
        },
        _getNumberOfMonths: function (e) {
            var t = this._get(e, "numberOfMonths");
            return t == null ? [1, 1] : typeof t == "number" ? [1, t] : t
        },
        _getMinMaxDate: function (e, t) {
            return this._determineDate(e, this._get(e, t + "Date"), null)
        },
        _getDaysInMonth: function (e, t) {
            return 32 - this._daylightSavingAdjust(new Date(e, t, 32)).getDate()
        },
        _getFirstDayOfMonth: function (e, t) {
            return (new Date(e, t, 1)).getDay()
        },
        _canAdjustMonth: function (e, t, n, r) {
            var i = this._getNumberOfMonths(e),
                s = this._daylightSavingAdjust(new Date(n, r + (t < 0 ? t : i[0] * i[1]), 1));
            return t < 0 && s.setDate(this._getDaysInMonth(s.getFullYear(), s.getMonth())), this._isInRange(e, s)
        },
        _isInRange: function (e, t) {
            var n, r, i = this._getMinMaxDate(e, "min"),
                s = this._getMinMaxDate(e, "max"),
                o = null,
                u = null,
                a = this._get(e, "yearRange");
            return a && (n = a.split(":"), r = (new Date).getFullYear(), o = parseInt(n[0], 10), u = parseInt(n[1], 10), n[0].match(/[+\-].*/) && (o += r), n[1].match(/[+\-].*/) && (u += r)), (!i || t.getTime() >= i.getTime()) && (!s || t.getTime() <= s.getTime()) && (!o || t.getFullYear() >= o) && (!u || t.getFullYear() <= u)
        },
        _getFormatConfig: function (e) {
            var t = this._get(e, "shortYearCutoff");
            return t = typeof t != "string" ? t : (new Date).getFullYear() % 100 + parseInt(t, 10), {
                shortYearCutoff: t,
                dayNamesShort: this._get(e, "dayNamesShort"),
                dayNames: this._get(e, "dayNames"),
                monthNamesShort: this._get(e, "monthNamesShort"),
                monthNames: this._get(e, "monthNames")
            }
        },
        _formatDate: function (e, t, n, r) {
            t || (e.currentDay = e.selectedDay, e.currentMonth = e.selectedMonth, e.currentYear = e.selectedYear);
            var i = t ? typeof t == "object" ? t : this._daylightSavingAdjust(new Date(r, n, t)) : this._daylightSavingAdjust(new Date(e.currentYear, e.currentMonth, e.currentDay));
            return this.formatDate(this._get(e, "dateFormat"), i, this._getFormatConfig(e))
        }
    }), e.fn.datepicker = function (t) {
        if (!this.length) return this;
        e.datepicker.initialized || (e(document).mousedown(e.datepicker._checkExternalClick), e.datepicker.initialized = !0), e("#" + e.datepicker._mainDivId).length === 0 && e("body").append(e.datepicker.dpDiv);
        var n = Array.prototype.slice.call(arguments, 1);
        return typeof t != "string" || t !== "isDisabled" && t !== "getDate" && t !== "widget" ? t === "option" && arguments.length === 2 && typeof arguments[1] == "string" ? e.datepicker["_" + t + "Datepicker"].apply(e.datepicker, [this[0]].concat(n)) : this.each(function () {
            typeof t == "string" ? e.datepicker["_" + t + "Datepicker"].apply(e.datepicker, [this].concat(n)) : e.datepicker._attachDatepicker(this, t)
        }) : e.datepicker["_" + t + "Datepicker"].apply(e.datepicker, [this[0]].concat(n))
    }, e.datepicker = new i, e.datepicker.initialized = !1, e.datepicker.uuid = (new Date).getTime(), e.datepicker.version = "1.10.4"
}(jQuery),
function (e) {
    e.cookie = function (t, n, r) {
        if (arguments.length > 1 && (!/Object/.test(Object.prototype.toString.call(n)) || n === null || n === undefined)) {
            r = e.extend({}, r);
            if (n === null || n === undefined) r.expires = -1;
            if (typeof r.expires == "number") {
                var i = r.expires,
                    s = r.expires = new Date;
                s.setDate(s.getDate() + i)
            }
            return n = String(n), document.cookie = [encodeURIComponent(t), "=", r.raw ? n : encodeURIComponent(n), r.expires ? "; expires=" + r.expires.toUTCString() : "", r.path ? "; path=" + r.path : "", r.domain ? "; domain=" + r.domain : "", r.secure ? "; secure" : ""].join("")
        }
        r = n || {};
        var o = r.raw ? function (e) {
            return e
        } : decodeURIComponent,
            u = document.cookie.split("; ");
        for (var a = 0, f; f = u[a] && u[a].split("=") ; a++)
            if (o(f[0]) === t) return o(f[1] || "");
        return null
    }
}(jQuery);
var festivals = {};
festivals["26/1/2014"] = "Republic Day", festivals["28/2/2014"] = "Maha Shivratri", festivals["17/3/2014"] = "Holi", festivals["18/4/2014"] = "Good festivalsriday", festivals["23/4/2014"] = "Mahavir Jayanthi", festivals["1/5/2014"] = "May Day", festivals["14/5/2014"] = "Buddha Purnima", festivals["15/8/2014"] = "Independence Day", festivals["17/8/2014"] = "Krishna Janmastami", festivals["29/8/2014"] = "Ganesh Chaturthi", festivals["2/10/2014"] = "Mahatma Gandhi Jayanthi", festivals["3/10/2014"] = "Vijaya Dashami", festivals["6/10/2014"] = "Iduâ€™lZuha", festivals["23/10/2014"] = "Diwali", festivals["6/11/2014"] = "Guru Nanak Jayanthi", festivals["25/12/2014"] = "Christmas", festivals["1/1/2015"] = "New Year's Day", festivals["4/1/2015"] = "Milad un-Nabi/Id-e-Milad", festivals["5/1/2015"] = "Guru Govind Singh Jayanti", festivals["15/1/2015"] = "Pongal", festivals["24/1/2015"] = "Vasant Panchami", festivals["26/1/2015"] = "Republic Day", festivals["3/2/2015"] = "Guru Ravidas Jayanti", festivals["14/2/2015"] = "Maharishi Dayanand Saraswati Jayanti", festivals["14/2/2015"] = "Valentine's Day", festivals["17/2/2015"] = "Maha Shivaratri/Shivaratri", festivals["19/2/2015"] = "Shivaji Jayanti", festivals["19/2/2015"] = "Chinese New Year", festivals["5/3/2015"] = "Holika Dahana", festivals["6/3/2015"] = "Dolyatra", festivals["20/3/2015"] = "March equinox", festivals["21/3/2015"] = "Chaitra Sukhladi", festivals["28/3/2015"] = "Rama Navami", festivals["2/4/2015"] = "Maundy Thursday", festivals["2/4/2015"] = "Mahavir Jayanti", festivals["3/4/2015"] = "Good Friday", festivals["4/4/2015"] = "First day of Passover", festivals["5/4/2015"] = "Easter Day", festivals["14/4/2015"] = "Vaisakhi", festivals["14/4/2015"] = "Ambedkar Jayanti", festivals["15/4/2015"] = "Mesadi/Vaisakhadi", festivals["1/5/2015"] = "May Day", festivals["3/5/2015"] = "Hazarat Ali's Birthday", festivals["4/5/2015"] = "Buddha Purnima/Vesak", festivals["9/5/2015"] = "Birthday of Ravindranath", festivals["10/5/2015"] = "Mother's Day", festivals["21/6/2015"] = "Father's Day", festivals["21/6/2015"] = "June Solstice", festivals["17/7/2015"] = "Jamat Ul-Vida", festivals["18/7/2015"] = "Rath Yatra", festivals["19/7/2015"] = "Ramzan Id/Eid-ul-Fitar", festivals["2/8/2015"] = "Friendship Day", festivals["15/8/2015"] = "Independence Day", festivals["15/8/2015"] = "Thanksgiving Day", festivals["18/8/2015"] = "Parsi New Year", festivals["28/8/2015"] = "Onam", festivals["29/8/2015"] = "Raksha Bandhan (Rakhi)", festivals["5/9/2015"] = "Janmashtami", festivals["17/9/2015"] = "Ganesh Chaturthi/Vinayaka Chaturthi", festivals["23/9/2015"] = "September equinox", festivals["25/9/2015"] = "Bakri Id/Eid ul-Adha", festivals["2/10/2015"] = "Mahatma Gandhi Jayanti", festivals["20/10/2015"] = "Maha Saptami", festivals["21/10/2015"] = "Maha Ashtami", festivals["22/10/2015"] = "Dussehra (Maha Navami)", festivals["24/10/2015"] = "Muharram/Ashura", festivals["27/10/2015"] = "Maharishi Valmiki Jayanti", festivals["30/10/2015"] = "Karaka Chaturthi (Karva Chauth)", festivals["31/10/2015"] = "Halloween", festivals["10/11/2015"] = "Naraka Chaturdasi", festivals["11/11/2015"] = "Diwali/Deepavali", festivals["12/11/2015"] = "Govardhan Puja", festivals["13/11/2015"] = "Bhai Duj", festivals["17/11/2015"] = "Chhat Puja (Pratihar Sashthi/Surya Sashthi)", festivals["24/11/2015"] = "Guru Tegh Bahadur's Martyrdom Day", festivals["25/11/2015"] = "Guru Nanak Jayanti", festivals["7/12/2015"] = "First Day of Hanukkah", festivals["14/12/2015"] = "Last day of Hanukkah", festivals["22/12/2015"] = "December Solstice", festivals["24/12/2015"] = "Christmas Eve", festivals["25/12/2015"] = "Christmas", festivals["31/12/2015"] = "New Year's Eve";
if ("undefined" == typeof jQuery) throw new Error("Bootstrap's JavaScript requires jQuery"); + function (e) {
    "use strict";

    function t() {
        var e = document.createElement("bootstrap"),
            t = {
                WebkitTransition: "webkitTransitionEnd",
                MozTransition: "transitionend",
                OTransition: "oTransitionEnd otransitionend",
                transition: "transitionend"
            };
        for (var n in t)
            if (void 0 !== e.style[n]) return {
                end: t[n]
            };
        return !1
    }
    e.fn.emulateTransitionEnd = function (t) {
        var n = !1,
            r = this;
        e(this).one(e.support.transition.end, function () {
            n = !0
        });
        var i = function () {
            n || e(r).trigger(e.support.transition.end)
        };
        return setTimeout(i, t), this
    }, e(function () {
        e.support.transition = t()
    })
}(jQuery), + function (e) {
    "use strict";
    var t = '[data-dismiss="alert"]',
        n = function (n) {
            e(n).on("click", t, this.close)
        };
    n.prototype.close = function (t) {
        function n() {
            s.trigger("closed.bs.alert").remove()
        }
        var r = e(this),
            i = r.attr("data-target");
        i || (i = r.attr("href"), i = i && i.replace(/.*(?=#[^\s]*$)/, ""));
        var s = e(i);
        t && t.preventDefault(), s.length || (s = r.hasClass("alert") ? r : r.parent()), s.trigger(t = e.Event("close.bs.alert")), t.isDefaultPrevented() || (s.removeClass("in"), e.support.transition && s.hasClass("fade") ? s.one(e.support.transition.end, n).emulateTransitionEnd(150) : n())
    };
    var r = e.fn.alert;
    e.fn.alert = function (t) {
        return this.each(function () {
            var r = e(this),
                i = r.data("bs.alert");
            i || r.data("bs.alert", i = new n(this)), "string" == typeof t && i[t].call(r)
        })
    }, e.fn.alert.Constructor = n, e.fn.alert.noConflict = function () {
        return e.fn.alert = r, this
    }, e(document).on("click.bs.alert.data-api", t, n.prototype.close)
}(jQuery), + function (e) {
    "use strict";
    var t = function (n, r) {
        this.$element = e(n), this.options = e.extend({}, t.DEFAULTS, r), this.isLoading = !1
    };
    t.DEFAULTS = {
        loadingText: "loading..."
    }, t.prototype.setState = function (t) {
        var n = "disabled",
            r = this.$element,
            i = r.is("input") ? "val" : "html",
            s = r.data();
        t += "Text", s.resetText || r.data("resetText", r[i]()), r[i](s[t] || this.options[t]), setTimeout(e.proxy(function () {
            "loadingText" == t ? (this.isLoading = !0, r.addClass(n).attr(n, n)) : this.isLoading && (this.isLoading = !1, r.removeClass(n).removeAttr(n))
        }, this), 0)
    }, t.prototype.toggle = function () {
        var e = !0,
            t = this.$element.closest('[data-toggle="buttons"]');
        if (t.length) {
            var n = this.$element.find("input");
            "radio" == n.prop("type") && (n.prop("checked") && this.$element.hasClass("active") ? e = !1 : t.find(".active").removeClass("active")), e && n.prop("checked", !this.$element.hasClass("active")).trigger("change")
        }
        e && this.$element.toggleClass("active")
    };
    var n = e.fn.button;
    e.fn.button = function (n) {
        return this.each(function () {
            var r = e(this),
                i = r.data("bs.button"),
                s = "object" == typeof n && n;
            i || r.data("bs.button", i = new t(this, s)), "toggle" == n ? i.toggle() : n && i.setState(n)
        })
    }, e.fn.button.Constructor = t, e.fn.button.noConflict = function () {
        return e.fn.button = n, this
    }, e(document).on("click.bs.button.data-api", "[data-toggle^=button]", function (t) {
        var n = e(t.target);
        n.hasClass("btn") || (n = n.closest(".btn")), n.button("toggle"), t.preventDefault()
    })
}(jQuery), + function (e) {
    "use strict";
    var t = function (t, n) {
        this.$element = e(t), this.$indicators = this.$element.find(".carousel-indicators"), this.options = n, this.paused = this.sliding = this.interval = this.$active = this.$items = null, "hover" == this.options.pause && this.$element.on("mouseenter", e.proxy(this.pause, this)).on("mouseleave", e.proxy(this.cycle, this))
    };
    t.DEFAULTS = {
        interval: 5e3,
        pause: "hover",
        wrap: !0
    }, t.prototype.cycle = function (t) {
        return t || (this.paused = !1), this.interval && clearInterval(this.interval), this.options.interval && !this.paused && (this.interval = setInterval(e.proxy(this.next, this), this.options.interval)), this
    }, t.prototype.getActiveIndex = function () {
        return this.$active = this.$element.find(".item.active"), this.$items = this.$active.parent().children(), this.$items.index(this.$active)
    }, t.prototype.to = function (t) {
        var n = this,
            r = this.getActiveIndex();
        return t > this.$items.length - 1 || 0 > t ? void 0 : this.sliding ? this.$element.one("slid.bs.carousel", function () {
            n.to(t)
        }) : r == t ? this.pause().cycle() : this.slide(t > r ? "next" : "prev", e(this.$items[t]))
    }, t.prototype.pause = function (t) {
        return t || (this.paused = !0), this.$element.find(".next, .prev").length && e.support.transition && (this.$element.trigger(e.support.transition.end), this.cycle(!0)), this.interval = clearInterval(this.interval), this
    }, t.prototype.next = function () {
        return this.sliding ? void 0 : this.slide("next")
    }, t.prototype.prev = function () {
        return this.sliding ? void 0 : this.slide("prev")
    }, t.prototype.slide = function (t, n) {
        var r = this.$element.find(".item.active"),
            i = n || r[t](),
            s = this.interval,
            o = "next" == t ? "left" : "right",
            u = "next" == t ? "first" : "last",
            f = this;
        if (!i.length) {
            if (!this.options.wrap) return;
            i = this.$element.find(".item")[u]()
        }
        if (i.hasClass("active")) return this.sliding = !1;
        var l = e.Event("slide.bs.carousel", {
            relatedTarget: i[0],
            direction: o
        });
        return this.$element.trigger(l), l.isDefaultPrevented() ? void 0 : (this.sliding = !0, s && this.pause(), this.$indicators.length && (this.$indicators.find(".active").removeClass("active"), this.$element.one("slid.bs.carousel", function () {
            var t = e(f.$indicators.children()[f.getActiveIndex()]);
            t && t.addClass("active")
        })), e.support.transition && this.$element.hasClass("slide") ? (i.addClass(t), i[0].offsetWidth, r.addClass(o), i.addClass(o), r.one(e.support.transition.end, function () {
            i.removeClass([t, o].join(" ")).addClass("active"), r.removeClass(["active", o].join(" ")), f.sliding = !1, setTimeout(function () {
                f.$element.trigger("slid.bs.carousel")
            }, 0)
        }).emulateTransitionEnd(1e3 * r.css("transition-duration").slice(0, -1))) : (r.removeClass("active"), i.addClass("active"), this.sliding = !1, this.$element.trigger("slid.bs.carousel")), s && this.cycle(), this)
    };
    var n = e.fn.carousel;
    e.fn.carousel = function (n) {
        return this.each(function () {
            var r = e(this),
                i = r.data("bs.carousel"),
                s = e.extend({}, t.DEFAULTS, r.data(), "object" == typeof n && n),
                o = "string" == typeof n ? n : s.slide;
            i || r.data("bs.carousel", i = new t(this, s)), "number" == typeof n ? i.to(n) : o ? i[o]() : s.interval && i.pause().cycle()
        })
    }, e.fn.carousel.Constructor = t, e.fn.carousel.noConflict = function () {
        return e.fn.carousel = n, this
    }, e(document).on("click.bs.carousel.data-api", "[data-slide], [data-slide-to]", function (t) {
        var n, r = e(this),
            i = e(r.attr("data-target") || (n = r.attr("href")) && n.replace(/.*(?=#[^\s]+$)/, "")),
            s = e.extend({}, i.data(), r.data()),
            o = r.attr("data-slide-to");
        o && (s.interval = !1), i.carousel(s), (o = r.attr("data-slide-to")) && i.data("bs.carousel").to(o), t.preventDefault()
    }), e(window).on("load", function () {
        e('[data-ride="carousel"]').each(function () {
            var t = e(this);
            t.carousel(t.data())
        })
    })
}(jQuery), + function (e) {
    "use strict";
    var t = function (n, r) {
        this.$element = e(n), this.options = e.extend({}, t.DEFAULTS, r), this.transitioning = null, this.options.parent && (this.$parent = e(this.options.parent)), this.options.toggle && this.toggle()
    };
    t.DEFAULTS = {
        toggle: !0
    }, t.prototype.dimension = function () {
        var e = this.$element.hasClass("width");
        return e ? "width" : "height"
    }, t.prototype.show = function () {
        if (!this.transitioning && !this.$element.hasClass("in")) {
            var t = e.Event("show.bs.collapse");
            if (this.$element.trigger(t), !t.isDefaultPrevented()) {
                var n = this.$parent && this.$parent.find("> .panel > .in");
                if (n && n.length) {
                    var r = n.data("bs.collapse");
                    if (r && r.transitioning) return;
                    n.collapse("hide"), r || n.data("bs.collapse", null)
                }
                var i = this.dimension();
                this.$element.removeClass("collapse").addClass("collapsing")[i](0), this.transitioning = 1;
                var s = function () {
                    this.$element.removeClass("collapsing").addClass("collapse in")[i]("auto"), this.transitioning = 0, this.$element.trigger("shown.bs.collapse")
                };
                if (!e.support.transition) return s.call(this);
                var o = e.camelCase(["scroll", i].join("-"));
                this.$element.one(e.support.transition.end, e.proxy(s, this)).emulateTransitionEnd(350)[i](this.$element[0][o])
            }
        }
    }, t.prototype.hide = function () {
        if (!this.transitioning && this.$element.hasClass("in")) {
            var t = e.Event("hide.bs.collapse");
            if (this.$element.trigger(t), !t.isDefaultPrevented()) {
                var n = this.dimension();
                this.$element[n](this.$element[n]())[0].offsetHeight, this.$element.addClass("collapsing").removeClass("collapse").removeClass("in"), this.transitioning = 1;
                var r = function () {
                    this.transitioning = 0, this.$element.trigger("hidden.bs.collapse").removeClass("collapsing").addClass("collapse")
                };
                return e.support.transition ? void this.$element[n](0).one(e.support.transition.end, e.proxy(r, this)).emulateTransitionEnd(350) : r.call(this)
            }
        }
    }, t.prototype.toggle = function () {
        this[this.$element.hasClass("in") ? "hide" : "show"]()
    };
    var n = e.fn.collapse;
    e.fn.collapse = function (n) {
        return this.each(function () {
            var r = e(this),
                i = r.data("bs.collapse"),
                s = e.extend({}, t.DEFAULTS, r.data(), "object" == typeof n && n);
            !i && s.toggle && "show" == n && (n = !n), i || r.data("bs.collapse", i = new t(this, s)), "string" == typeof n && i[n]()
        })
    }, e.fn.collapse.Constructor = t, e.fn.collapse.noConflict = function () {
        return e.fn.collapse = n, this
    }, e(document).on("click.bs.collapse.data-api", "[data-toggle=collapse]", function (t) {
        var n, r = e(this),
            i = r.attr("data-target") || t.preventDefault() || (n = r.attr("href")) && n.replace(/.*(?=#[^\s]+$)/, ""),
            s = e(i),
            o = s.data("bs.collapse"),
            u = o ? "toggle" : r.data(),
            f = r.attr("data-parent"),
            l = f && e(f);
        o && o.transitioning || (l && l.find('[data-toggle=collapse][data-parent="' + f + '"]').not(r).addClass("collapsed"), r[s.hasClass("in") ? "addClass" : "removeClass"]("collapsed")), s.collapse(u)
    })
}(jQuery), + function (e) {
    "use strict";

    function t(t) {
        e(r).remove(), e(i).each(function () {
            var r = n(e(this)),
                i = {
                    relatedTarget: this
                };
            r.hasClass("open") && (r.trigger(t = e.Event("hide.bs.dropdown", i)), t.isDefaultPrevented() || r.removeClass("open").trigger("hidden.bs.dropdown", i))
        })
    }

    function n(t) {
        var n = t.attr("data-target");
        n || (n = t.attr("href"), n = n && /#[A-Za-z]/.test(n) && n.replace(/.*(?=#[^\s]*$)/, ""));
        var r = n && e(n);
        return r && r.length ? r : t.parent()
    }
    var r = ".dropdown-backdrop",
        i = "[data-toggle=dropdown]",
        s = function (t) {
            e(t).on("click.bs.dropdown", this.toggle)
        };
    s.prototype.toggle = function (r) {
        var i = e(this);
        if (!i.is(".disabled, :disabled")) {
            var s = n(i),
                o = s.hasClass("open");
            if (t(), !o) {
                "ontouchstart" in document.documentElement && !s.closest(".navbar-nav").length && e('<div class="dropdown-backdrop"/>').insertAfter(e(this)).on("click", t);
                var u = {
                    relatedTarget: this
                };
                if (s.trigger(r = e.Event("show.bs.dropdown", u)), r.isDefaultPrevented()) return;
                s.toggleClass("open").trigger("shown.bs.dropdown", u), i.focus()
            }
            return !1
        }
    }, s.prototype.keydown = function (t) {
        if (/(38|40|27)/.test(t.keyCode)) {
            var r = e(this);
            if (t.preventDefault(), t.stopPropagation(), !r.is(".disabled, :disabled")) {
                var s = n(r),
                    o = s.hasClass("open");
                if (!o || o && 27 == t.keyCode) return 27 == t.which && s.find(i).focus(), r.click();
                var u = " li:not(.divider):visible a",
                    f = s.find("[role=menu]" + u + ", [role=listbox]" + u);
                if (f.length) {
                    var l = f.index(f.filter(":focus"));
                    38 == t.keyCode && l > 0 && l--, 40 == t.keyCode && l < f.length - 1 && l++, ~l || (l = 0), f.eq(l).focus()
                }
            }
        }
    };
    var o = e.fn.dropdown;
    e.fn.dropdown = function (t) {
        return this.each(function () {
            var n = e(this),
                r = n.data("bs.dropdown");
            r || n.data("bs.dropdown", r = new s(this)), "string" == typeof t && r[t].call(n)
        })
    }, e.fn.dropdown.Constructor = s, e.fn.dropdown.noConflict = function () {
        return e.fn.dropdown = o, this
    }, e(document).on("click.bs.dropdown.data-api", t).on("click.bs.dropdown.data-api", ".dropdown form", function (e) {
        e.stopPropagation()
    }).on("click.bs.dropdown.data-api", i, s.prototype.toggle).on("keydown.bs.dropdown.data-api", i + ", [role=menu], [role=listbox]", s.prototype.keydown)
}(jQuery), + function (e) {
    "use strict";
    var t = function (t, n) {
        this.options = n, this.$element = e(t), this.$backdrop = this.isShown = null, this.options.remote && this.$element.find(".modal-content").load(this.options.remote, e.proxy(function () {
            this.$element.trigger("loaded.bs.modal")
        }, this))
    };
    t.DEFAULTS = {
        backdrop: !0,
        keyboard: !0,
        show: !0
    }, t.prototype.toggle = function (e) {
        return this[this.isShown ? "hide" : "show"](e)
    }, t.prototype.show = function (t) {
        var n = this,
            r = e.Event("show.bs.modal", {
                relatedTarget: t
            });
        this.$element.trigger(r), this.isShown || r.isDefaultPrevented() || (this.isShown = !0, this.escape(), this.$element.on("click.dismiss.bs.modal", '[data-dismiss="modal"]', e.proxy(this.hide, this)), this.backdrop(function () {
            var r = e.support.transition && n.$element.hasClass("fade");
            n.$element.parent().length || n.$element.appendTo(document.body), n.$element.show().scrollTop(0), r && n.$element[0].offsetWidth, n.$element.addClass("in").attr("aria-hidden", !1), n.enforceFocus();
            var i = e.Event("shown.bs.modal", {
                relatedTarget: t
            });
            r ? n.$element.find(".modal-dialog").one(e.support.transition.end, function () {
                n.$element.focus().trigger(i)
            }).emulateTransitionEnd(300) : n.$element.focus().trigger(i)
        }))
    }, t.prototype.hide = function (t) {
        t && t.preventDefault(), t = e.Event("hide.bs.modal"), this.$element.trigger(t), this.isShown && !t.isDefaultPrevented() && (this.isShown = !1, this.escape(), e(document).off("focusin.bs.modal"), this.$element.removeClass("in").attr("aria-hidden", !0).off("click.dismiss.bs.modal"), e.support.transition && this.$element.hasClass("fade") ? this.$element.one(e.support.transition.end, e.proxy(this.hideModal, this)).emulateTransitionEnd(300) : this.hideModal())
    }, t.prototype.enforceFocus = function () {
        e(document).off("focusin.bs.modal").on("focusin.bs.modal", e.proxy(function (e) {
            this.$element[0] === e.target || this.$element.has(e.target).length || this.$element.focus()
        }, this))
    }, t.prototype.escape = function () {
        this.isShown && this.options.keyboard ? this.$element.on("keyup.dismiss.bs.modal", e.proxy(function (e) {
            27 == e.which && this.hide()
        }, this)) : this.isShown || this.$element.off("keyup.dismiss.bs.modal")
    }, t.prototype.hideModal = function () {
        var e = this;
        this.$element.hide(), this.backdrop(function () {
            e.removeBackdrop(), e.$element.trigger("hidden.bs.modal")
        })
    }, t.prototype.removeBackdrop = function () {
        this.$backdrop && this.$backdrop.remove(), this.$backdrop = null
    }, t.prototype.backdrop = function (t) {
        var n = this.$element.hasClass("fade") ? "fade" : "";
        if (this.isShown && this.options.backdrop) {
            var r = e.support.transition && n;
            if (this.$backdrop = e('<div class="modal-backdrop ' + n + '" />').appendTo(document.body), this.$element.on("click.dismiss.bs.modal", e.proxy(function (e) {
                    e.target === e.currentTarget && ("static" == this.options.backdrop ? this.$element[0].focus.call(this.$element[0]) : this.hide.call(this))
            }, this)), r && this.$backdrop[0].offsetWidth, this.$backdrop.addClass("in"), !t) return;
            r ? this.$backdrop.one(e.support.transition.end, t).emulateTransitionEnd(150) : t()
        } else !this.isShown && this.$backdrop ? (this.$backdrop.removeClass("in"), e.support.transition && this.$element.hasClass("fade") ? this.$backdrop.one(e.support.transition.end, t).emulateTransitionEnd(150) : t()) : t && t()
    };
    var n = e.fn.modal;
    e.fn.modal = function (n, r) {
        return this.each(function () {
            var i = e(this),
                s = i.data("bs.modal"),
                o = e.extend({}, t.DEFAULTS, i.data(), "object" == typeof n && n);
            s || i.data("bs.modal", s = new t(this, o)), "string" == typeof n ? s[n](r) : o.show && s.show(r)
        })
    }, e.fn.modal.Constructor = t, e.fn.modal.noConflict = function () {
        return e.fn.modal = n, this
    }, e(document).on("click.bs.modal.data-api", '[data-toggle="modal"]', function (t) {
        var n = e(this),
            r = n.attr("href"),
            i = e(n.attr("data-target") || r && r.replace(/.*(?=#[^\s]+$)/, "")),
            s = i.data("bs.modal") ? "toggle" : e.extend({
                remote: !/#/.test(r) && r
            }, i.data(), n.data());
        n.is("a") && t.preventDefault(), i.modal(s, this).one("hide", function () {
            n.is(":visible") && n.focus()
        })
    }), e(document).on("show.bs.modal", ".modal", function () {
        e(document.body).addClass("modal-open")
    }).on("hidden.bs.modal", ".modal", function () {
        e(document.body).removeClass("modal-open")
    })
}(jQuery), + function (e) {
    "use strict";
    var t = function (e, t) {
        this.type = this.options = this.enabled = this.timeout = this.hoverState = this.$element = null, this.init("tooltip", e, t)
    };
    t.DEFAULTS = {
        animation: !0,
        placement: "top",
        selector: !1,
        template: '<div class="tooltip"><div class="tooltip-arrow"></div><div class="tooltip-inner"></div></div>',
        trigger: "hover focus",
        title: "",
        delay: 0,
        html: !1,
        container: !1
    }, t.prototype.init = function (t, n, r) {
        this.enabled = !0, this.type = t, this.$element = e(n), this.options = this.getOptions(r);
        for (var i = this.options.trigger.split(" "), s = i.length; s--;) {
            var o = i[s];
            if ("click" == o) this.$element.on("click." + this.type, this.options.selector, e.proxy(this.toggle, this));
            else if ("manual" != o) {
                var u = "hover" == o ? "mouseenter" : "focusin",
                    f = "hover" == o ? "mouseleave" : "focusout";
                this.$element.on(u + "." + this.type, this.options.selector, e.proxy(this.enter, this)), this.$element.on(f + "." + this.type, this.options.selector, e.proxy(this.leave, this))
            }
        }
        this.options.selector ? this._options = e.extend({}, this.options, {
            trigger: "manual",
            selector: ""
        }) : this.fixTitle()
    }, t.prototype.getDefaults = function () {
        return t.DEFAULTS
    }, t.prototype.getOptions = function (t) {
        return t = e.extend({}, this.getDefaults(), this.$element.data(), t), t.delay && "number" == typeof t.delay && (t.delay = {
            show: t.delay,
            hide: t.delay
        }), t
    }, t.prototype.getDelegateOptions = function () {
        var t = {},
            n = this.getDefaults();
        return this._options && e.each(this._options, function (e, r) {
            n[e] != r && (t[e] = r)
        }), t
    }, t.prototype.enter = function (t) {
        var n = t instanceof this.constructor ? t : e(t.currentTarget)[this.type](this.getDelegateOptions()).data("bs." + this.type);
        return clearTimeout(n.timeout), n.hoverState = "in", n.options.delay && n.options.delay.show ? void (n.timeout = setTimeout(function () {
            "in" == n.hoverState && n.show()
        }, n.options.delay.show)) : n.show()
    }, t.prototype.leave = function (t) {
        var n = t instanceof this.constructor ? t : e(t.currentTarget)[this.type](this.getDelegateOptions()).data("bs." + this.type);
        return clearTimeout(n.timeout), n.hoverState = "out", n.options.delay && n.options.delay.hide ? void (n.timeout = setTimeout(function () {
            "out" == n.hoverState && n.hide()
        }, n.options.delay.hide)) : n.hide()
    }, t.prototype.show = function () {
        var t = e.Event("show.bs." + this.type);
        if (this.hasContent() && this.enabled) {
            if (this.$element.trigger(t), t.isDefaultPrevented()) return;
            var n = this,
                r = this.tip();
            this.setContent(), this.options.animation && r.addClass("fade");
            var i = "function" == typeof this.options.placement ? this.options.placement.call(this, r[0], this.$element[0]) : this.options.placement,
                s = /\s?auto?\s?/i,
                o = s.test(i);
            o && (i = i.replace(s, "") || "top"), r.detach().css({
                top: 0,
                left: 0,
                display: "block"
            }).addClass(i), this.options.container ? r.appendTo(this.options.container) : r.insertAfter(this.$element);
            var u = this.getPosition(),
                f = r[0].offsetWidth,
                l = r[0].offsetHeight;
            if (o) {
                var c = this.$element.parent(),
                    h = i,
                    p = document.documentElement.scrollTop || document.body.scrollTop,
                    d = "body" == this.options.container ? window.innerWidth : c.outerWidth(),
                    v = "body" == this.options.container ? window.innerHeight : c.outerHeight(),
                    m = "body" == this.options.container ? 0 : c.offset().left;
                i = "bottom" == i && u.top + u.height + l - p > v ? "top" : "top" == i && u.top - p - l < 0 ? "bottom" : "right" == i && u.right + f > d ? "left" : "left" == i && u.left - f < m ? "right" : i, r.removeClass(h).addClass(i)
            }
            var g = this.getCalculatedOffset(i, u, f, l);
            this.applyPlacement(g, i), this.hoverState = null;
            var y = function () {
                n.$element.trigger("shown.bs." + n.type)
            };
            e.support.transition && this.$tip.hasClass("fade") ? r.one(e.support.transition.end, y).emulateTransitionEnd(150) : y()
        }
    }, t.prototype.applyPlacement = function (t, n) {
        var r, i = this.tip(),
            s = i[0].offsetWidth,
            o = i[0].offsetHeight,
            u = parseInt(i.css("margin-top"), 10),
            f = parseInt(i.css("margin-left"), 10);
        isNaN(u) && (u = 0), isNaN(f) && (f = 0), t.top = t.top + u, t.left = t.left + f, e.offset.setOffset(i[0], e.extend({
            using: function (e) {
                i.css({
                    top: Math.round(e.top),
                    left: Math.round(e.left)
                })
            }
        }, t), 0), i.addClass("in");
        var l = i[0].offsetWidth,
            c = i[0].offsetHeight;
        if ("top" == n && c != o && (r = !0, t.top = t.top + o - c), /bottom|top/.test(n)) {
            var h = 0;
            t.left < 0 && (h = -2 * t.left, t.left = 0, i.offset(t), l = i[0].offsetWidth, c = i[0].offsetHeight), this.replaceArrow(h - s + l, l, "left")
        } else this.replaceArrow(c - o, c, "top");
        r && i.offset(t)
    }, t.prototype.replaceArrow = function (e, t, n) {
        this.arrow().css(n, e ? 50 * (1 - e / t) + "%" : "")
    }, t.prototype.setContent = function () {
        var e = this.tip(),
            t = this.getTitle();
        e.find(".tooltip-inner")[this.options.html ? "html" : "text"](t), e.removeClass("fade in top bottom left right")
    }, t.prototype.hide = function () {
        function t() {
            "in" != n.hoverState && r.detach(), n.$element.trigger("hidden.bs." + n.type)
        }
        var n = this,
            r = this.tip(),
            i = e.Event("hide.bs." + this.type);
        return this.$element.trigger(i), i.isDefaultPrevented() ? void 0 : (r.removeClass("in"), e.support.transition && this.$tip.hasClass("fade") ? r.one(e.support.transition.end, t).emulateTransitionEnd(150) : t(), this.hoverState = null, this)
    }, t.prototype.fixTitle = function () {
        var e = this.$element;
        (e.attr("title") || "string" != typeof e.attr("data-original-title")) && e.attr("data-original-title", e.attr("title") || "").attr("title", "")
    }, t.prototype.hasContent = function () {
        return this.getTitle()
    }, t.prototype.getPosition = function () {
        var t = this.$element[0];
        return e.extend({}, "function" == typeof t.getBoundingClientRect ? t.getBoundingClientRect() : {
            width: t.offsetWidth,
            height: t.offsetHeight
        }, this.$element.offset())
    }, t.prototype.getCalculatedOffset = function (e, t, n, r) {
        return "bottom" == e ? {
            top: t.top + t.height,
            left: t.left + t.width / 2 - n / 2
        } : "top" == e ? {
            top: t.top - r,
            left: t.left + t.width / 2 - n / 2
        } : "left" == e ? {
            top: t.top + t.height / 2 - r / 2,
            left: t.left - n
        } : {
            top: t.top + t.height / 2 - r / 2,
            left: t.left + t.width
        }
    }, t.prototype.getTitle = function () {
        var e, t = this.$element,
            n = this.options;
        return e = t.attr("data-original-title") || ("function" == typeof n.title ? n.title.call(t[0]) : n.title)
    }, t.prototype.tip = function () {
        return this.$tip = this.$tip || e(this.options.template)
    }, t.prototype.arrow = function () {
        return this.$arrow = this.$arrow || this.tip().find(".tooltip-arrow")
    }, t.prototype.validate = function () {
        this.$element[0].parentNode || (this.hide(), this.$element = null, this.options = null)
    }, t.prototype.enable = function () {
        this.enabled = !0
    }, t.prototype.disable = function () {
        this.enabled = !1
    }, t.prototype.toggleEnabled = function () {
        this.enabled = !this.enabled
    }, t.prototype.toggle = function (t) {
        var n = t ? e(t.currentTarget)[this.type](this.getDelegateOptions()).data("bs." + this.type) : this;
        n.tip().hasClass("in") ? n.leave(n) : n.enter(n)
    }, t.prototype.destroy = function () {
        clearTimeout(this.timeout), this.hide().$element.off("." + this.type).removeData("bs." + this.type)
    };
    var n = e.fn.tooltip;
    e.fn.tooltip = function (n) {
        return this.each(function () {
            var r = e(this),
                i = r.data("bs.tooltip"),
                s = "object" == typeof n && n;
            (i || "destroy" != n) && (i || r.data("bs.tooltip", i = new t(this, s)), "string" == typeof n && i[n]())
        })
    }, e.fn.tooltip.Constructor = t, e.fn.tooltip.noConflict = function () {
        return e.fn.tooltip = n, this
    }
}(jQuery), + function (e) {
    "use strict";
    var t = function (e, t) {
        this.init("popover", e, t)
    };
    if (!e.fn.tooltip) throw new Error("Popover requires tooltip.js");
    t.DEFAULTS = e.extend({}, e.fn.tooltip.Constructor.DEFAULTS, {
        placement: "right",
        trigger: "click",
        content: "",
        template: '<div class="popover"><div class="arrow"></div><h3 class="popover-title"></h3><div class="popover-content"></div></div>'
    }), t.prototype = e.extend({}, e.fn.tooltip.Constructor.prototype), t.prototype.constructor = t, t.prototype.getDefaults = function () {
        return t.DEFAULTS
    }, t.prototype.setContent = function () {
        var e = this.tip(),
            t = this.getTitle(),
            n = this.getContent();
        e.find(".popover-title")[this.options.html ? "html" : "text"](t), e.find(".popover-content")[this.options.html ? "string" == typeof n ? "html" : "append" : "text"](n), e.removeClass("fade top bottom left right in"), e.find(".popover-title").html() || e.find(".popover-title").hide()
    }, t.prototype.hasContent = function () {
        return this.getTitle() || this.getContent()
    }, t.prototype.getContent = function () {
        var e = this.$element,
            t = this.options;
        return e.attr("data-content") || ("function" == typeof t.content ? t.content.call(e[0]) : t.content)
    }, t.prototype.arrow = function () {
        return this.$arrow = this.$arrow || this.tip().find(".arrow")
    }, t.prototype.tip = function () {
        return this.$tip || (this.$tip = e(this.options.template)), this.$tip
    };
    var n = e.fn.popover;
    e.fn.popover = function (n) {
        return this.each(function () {
            var r = e(this),
                i = r.data("bs.popover"),
                s = "object" == typeof n && n;
            (i || "destroy" != n) && (i || r.data("bs.popover", i = new t(this, s)), "string" == typeof n && i[n]())
        })
    }, e.fn.popover.Constructor = t, e.fn.popover.noConflict = function () {
        return e.fn.popover = n, this
    }
}(jQuery), + function (e) {
    "use strict";

    function t(n, r) {
        var i, s = e.proxy(this.process, this);
        this.$element = e(e(n).is("body") ? window : n), this.$body = e("body"), this.$scrollElement = this.$element.on("scroll.bs.scroll-spy.data-api", s), this.options = e.extend({}, t.DEFAULTS, r), this.selector = (this.options.target || (i = e(n).attr("href")) && i.replace(/.*(?=#[^\s]+$)/, "") || "") + " .nav li > a", this.offsets = e([]), this.targets = e([]), this.activeTarget = null, this.refresh(), this.process()
    }
    t.DEFAULTS = {
        offset: 10
    }, t.prototype.refresh = function () {
        var t = this.$element[0] == window ? "offset" : "position";
        this.offsets = e([]), this.targets = e([]);
        var n = this;
        this.$body.find(this.selector).map(function () {
            var r = e(this),
                i = r.data("target") || r.attr("href"),
                s = /^#./.test(i) && e(i);
            return s && s.length && s.is(":visible") && [
                [s[t]().top + (!e.isWindow(n.$scrollElement.get(0)) && n.$scrollElement.scrollTop()), i]
            ] || null
        }).sort(function (e, t) {
            return e[0] - t[0]
        }).each(function () {
            n.offsets.push(this[0]), n.targets.push(this[1])
        })
    }, t.prototype.process = function () {
        var e, t = this.$scrollElement.scrollTop() + this.options.offset,
            n = this.$scrollElement[0].scrollHeight || this.$body[0].scrollHeight,
            r = n - this.$scrollElement.height(),
            i = this.offsets,
            s = this.targets,
            o = this.activeTarget;
        if (t >= r) return o != (e = s.last()[0]) && this.activate(e);
        if (o && t <= i[0]) return o != (e = s[0]) && this.activate(e);
        for (e = i.length; e--;) o != s[e] && t >= i[e] && (!i[e + 1] || t <= i[e + 1]) && this.activate(s[e])
    }, t.prototype.activate = function (t) {
        this.activeTarget = t, e(this.selector).parentsUntil(this.options.target, ".active").removeClass("active");
        var n = this.selector + '[data-target="' + t + '"],' + this.selector + '[href="' + t + '"]',
            r = e(n).parents("li").addClass("active");
        r.parent(".dropdown-menu").length && (r = r.closest("li.dropdown").addClass("active")), r.trigger("activate.bs.scrollspy")
    };
    var n = e.fn.scrollspy;
    e.fn.scrollspy = function (n) {
        return this.each(function () {
            var r = e(this),
                i = r.data("bs.scrollspy"),
                s = "object" == typeof n && n;
            i || r.data("bs.scrollspy", i = new t(this, s)), "string" == typeof n && i[n]()
        })
    }, e.fn.scrollspy.Constructor = t, e.fn.scrollspy.noConflict = function () {
        return e.fn.scrollspy = n, this
    }, e(window).on("load", function () {
        e('[data-spy="scroll"]').each(function () {
            var t = e(this);
            t.scrollspy(t.data())
        })
    })
}(jQuery), + function (e) {
    "use strict";
    var t = function (t) {
        this.element = e(t)
    };
    t.prototype.show = function () {
        var t = this.element,
            n = t.closest("ul:not(.dropdown-menu)"),
            r = t.data("target");
        if (r || (r = t.attr("href"), r = r && r.replace(/.*(?=#[^\s]*$)/, "")), !t.parent("li").hasClass("active")) {
            var i = n.find(".active:last a")[0],
                s = e.Event("show.bs.tab", {
                    relatedTarget: i
                });
            if (t.trigger(s), !s.isDefaultPrevented()) {
                var o = e(r);
                this.activate(t.parent("li"), n), this.activate(o, o.parent(), function () {
                    t.trigger({
                        type: "shown.bs.tab",
                        relatedTarget: i
                    })
                })
            }
        }
    }, t.prototype.activate = function (t, n, r) {
        function i() {
            s.removeClass("active").find("> .dropdown-menu > .active").removeClass("active"), t.addClass("active"), o ? (t[0].offsetWidth, t.addClass("in")) : t.removeClass("fade"), t.parent(".dropdown-menu") && t.closest("li.dropdown").addClass("active"), r && r()
        }
        var s = n.find("> .active"),
            o = r && e.support.transition && s.hasClass("fade");
        o ? s.one(e.support.transition.end, i).emulateTransitionEnd(150) : i(), s.removeClass("in")
    };
    var n = e.fn.tab;
    e.fn.tab = function (n) {
        return this.each(function () {
            var r = e(this),
                i = r.data("bs.tab");
            i || r.data("bs.tab", i = new t(this)), "string" == typeof n && i[n]()
        })
    }, e.fn.tab.Constructor = t, e.fn.tab.noConflict = function () {
        return e.fn.tab = n, this
    }, e(document).on("click.bs.tab.data-api", '[data-toggle="tab"], [data-toggle="pill"]', function (t) {
        t.preventDefault(), e(this).tab("show")
    })
}(jQuery), + function (e) {
    "use strict";
    var t = function (n, r) {
        this.options = e.extend({}, t.DEFAULTS, r), this.$window = e(window).on("scroll.bs.affix.data-api", e.proxy(this.checkPosition, this)).on("click.bs.affix.data-api", e.proxy(this.checkPositionWithEventLoop, this)), this.$element = e(n), this.affixed = this.unpin = this.pinnedOffset = null, this.checkPosition()
    };
    t.RESET = "affix affix-top affix-bottom", t.DEFAULTS = {
        offset: 0
    }, t.prototype.getPinnedOffset = function () {
        if (this.pinnedOffset) return this.pinnedOffset;
        this.$element.removeClass(t.RESET).addClass("affix");
        var e = this.$window.scrollTop(),
            n = this.$element.offset();
        return this.pinnedOffset = n.top - e
    }, t.prototype.checkPositionWithEventLoop = function () {
        setTimeout(e.proxy(this.checkPosition, this), 1)
    }, t.prototype.checkPosition = function () {
        if (this.$element.is(":visible")) {
            var n = e(document).height(),
                r = this.$window.scrollTop(),
                i = this.$element.offset(),
                s = this.options.offset,
                o = s.top,
                u = s.bottom;
            "top" == this.affixed && (i.top += r), "object" != typeof s && (u = o = s), "function" == typeof o && (o = s.top(this.$element)), "function" == typeof u && (u = s.bottom(this.$element));
            var f = null != this.unpin && r + this.unpin <= i.top ? !1 : null != u && i.top + this.$element.height() >= n - u ? "bottom" : null != o && o >= r ? "top" : !1;
            if (this.affixed !== f) {
                this.unpin && this.$element.css("top", "");
                var l = "affix" + (f ? "-" + f : ""),
                    c = e.Event(l + ".bs.affix");
                this.$element.trigger(c), c.isDefaultPrevented() || (this.affixed = f, this.unpin = "bottom" == f ? this.getPinnedOffset() : null, this.$element.removeClass(t.RESET).addClass(l).trigger(e.Event(l.replace("affix", "affixed"))), "bottom" == f && this.$element.offset({
                    top: n - u - this.$element.height()
                }))
            }
        }
    };
    var n = e.fn.affix;
    e.fn.affix = function (n) {
        return this.each(function () {
            var r = e(this),
                i = r.data("bs.affix"),
                s = "object" == typeof n && n;
            i || r.data("bs.affix", i = new t(this, s)), "string" == typeof n && i[n]()
        })
    }, e.fn.affix.Constructor = t, e.fn.affix.noConflict = function () {
        return e.fn.affix = n, this
    }, e(window).on("load", function () {
        e('[data-spy="affix"]').each(function () {
            var t = e(this),
                n = t.data();
            n.offset = n.offset || {}, n.offsetBottom && (n.offset.bottom = n.offsetBottom), n.offsetTop && (n.offset.top = n.offsetTop), t.affix(n)
        })
    })
}(jQuery);
var deviceIsAndroid = navigator.userAgent.indexOf("Android") > 0,
    deviceIsIOS = /iP(ad|hone|od)/.test(navigator.userAgent),
    deviceIsIOS4 = deviceIsIOS && /OS 4_\d(_\d)?/.test(navigator.userAgent),
    deviceIsIOSWithBadTarget = deviceIsIOS && /OS ([6-9]|\d{2})_\d/.test(navigator.userAgent);
FastClick.prototype.needsClick = function (e) {
    "use strict";
    switch (e.nodeName.toLowerCase()) {
        case "button":
        case "select":
        case "textarea":
            if (e.disabled) return !0;
            break;
        case "input":
            if (deviceIsIOS && e.type === "file" || e.disabled) return !0;
            break;
        case "label":
        case "video":
            return !0
    }
    return /\bneedsclick\b/.test(e.className)
}, FastClick.prototype.needsFocus = function (e) {
    "use strict";
    switch (e.nodeName.toLowerCase()) {
        case "textarea":
            return !0;
        case "select":
            return !deviceIsAndroid;
        case "input":
            switch (e.type) {
                case "button":
                case "checkbox":
                case "file":
                case "image":
                case "radio":
                case "submit":
                    return !1
            }
            return !e.disabled && !e.readOnly;
        default:
            return /\bneedsfocus\b/.test(e.className)
    }
}, FastClick.prototype.sendClick = function (e, t) {
    "use strict";
    var n, r;
    document.activeElement && document.activeElement !== e && document.activeElement.blur(), r = t.changedTouches[0], n = document.createEvent("MouseEvents"), n.initMouseEvent(this.determineEventType(e), !0, !0, window, 1, r.screenX, r.screenY, r.clientX, r.clientY, !1, !1, !1, !1, 0, null), n.forwardedTouchEvent = !0, e.dispatchEvent(n)
}, FastClick.prototype.determineEventType = function (e) {
    "use strict";
    return deviceIsAndroid && e.tagName.toLowerCase() === "select" ? "mousedown" : "click"
}, FastClick.prototype.focus = function (e) {
    "use strict";
    var t;
    deviceIsIOS && e.setSelectionRange && e.type.indexOf("date") !== 0 && e.type !== "time" ? (t = e.value.length, e.setSelectionRange(t, t)) : e.focus()
}, FastClick.prototype.updateScrollParent = function (e) {
    "use strict";
    var t, n;
    t = e.fastClickScrollParent;
    if (!t || !t.contains(e)) {
        n = e;
        do {
            if (n.scrollHeight > n.offsetHeight) {
                t = n, e.fastClickScrollParent = n;
                break
            }
            n = n.parentElement
        } while (n)
    }
    t && (t.fastClickLastScrollTop = t.scrollTop)
}, FastClick.prototype.getTargetElementFromEventTarget = function (e) {
    "use strict";
    return e.nodeType === Node.TEXT_NODE ? e.parentNode : e
}, FastClick.prototype.onTouchStart = function (e) {
    "use strict";
    var t, n, r;
    if (e.targetTouches.length > 1) return !0;
    t = this.getTargetElementFromEventTarget(e.target), n = e.targetTouches[0];
    if (deviceIsIOS) {
        r = window.getSelection();
        if (r.rangeCount && !r.isCollapsed) return !0;
        if (!deviceIsIOS4) {
            if (n.identifier === this.lastTouchIdentifier) return e.preventDefault(), !1;
            this.lastTouchIdentifier = n.identifier, this.updateScrollParent(t)
        }
    }
    return this.trackingClick = !0, this.trackingClickStart = e.timeStamp, this.targetElement = t, this.touchStartX = n.pageX, this.touchStartY = n.pageY, e.timeStamp - this.lastClickTime < this.tapDelay && e.preventDefault(), !0
}, FastClick.prototype.touchHasMoved = function (e) {
    "use strict";
    var t = e.changedTouches[0],
        n = this.touchBoundary;
    return Math.abs(t.pageX - this.touchStartX) > n || Math.abs(t.pageY - this.touchStartY) > n ? !0 : !1
}, FastClick.prototype.onTouchMove = function (e) {
    "use strict";
    if (!this.trackingClick) return !0;
    if (this.targetElement !== this.getTargetElementFromEventTarget(e.target) || this.touchHasMoved(e)) this.trackingClick = !1, this.targetElement = null;
    return !0
}, FastClick.prototype.findControl = function (e) {
    "use strict";
    return e.control !== undefined ? e.control : e.htmlFor ? document.getElementById(e.htmlFor) : e.querySelector("button, input:not([type=hidden]), keygen, meter, output, progress, select, textarea")
}, FastClick.prototype.onTouchEnd = function (e) {
    "use strict";
    var t, n, r, i, s, o = this.targetElement;
    if (!this.trackingClick) return !0;
    if (e.timeStamp - this.lastClickTime < this.tapDelay) return this.cancelNextClick = !0, !0;
    this.cancelNextClick = !1, this.lastClickTime = e.timeStamp, n = this.trackingClickStart, this.trackingClick = !1, this.trackingClickStart = 0, deviceIsIOSWithBadTarget && (s = e.changedTouches[0], o = document.elementFromPoint(s.pageX - window.pageXOffset, s.pageY - window.pageYOffset) || o, o.fastClickScrollParent = this.targetElement.fastClickScrollParent), r = o.tagName.toLowerCase();
    if (r === "label") {
        t = this.findControl(o);
        if (t) {
            this.focus(o);
            if (deviceIsAndroid) return !1;
            o = t
        }
    } else if (this.needsFocus(o)) {
        if (e.timeStamp - n > 100 || deviceIsIOS && window.top !== window && r === "input") return this.targetElement = null, !1;
        this.focus(o), this.sendClick(o, e);
        if (!deviceIsIOS || r !== "select") this.targetElement = null, e.preventDefault();
        return !1
    }
    if (deviceIsIOS && !deviceIsIOS4) {
        i = o.fastClickScrollParent;
        if (i && i.fastClickLastScrollTop !== i.scrollTop) return !0
    }
    return this.needsClick(o) || (e.preventDefault(), this.sendClick(o, e)), !1
}, FastClick.prototype.onTouchCancel = function () {
    "use strict";
    this.trackingClick = !1, this.targetElement = null
}, FastClick.prototype.onMouse = function (e) {
    "use strict";
    return this.targetElement ? e.forwardedTouchEvent ? !0 : e.cancelable ? !this.needsClick(this.targetElement) || this.cancelNextClick ? (e.stopImmediatePropagation ? e.stopImmediatePropagation() : e.propagationStopped = !0, e.stopPropagation(), e.preventDefault(), !1) : !0 : !0 : !0
}, FastClick.prototype.onClick = function (e) {
    "use strict";
    var t;
    return this.trackingClick ? (this.targetElement = null, this.trackingClick = !1, !0) : e.target.type === "submit" && e.detail === 0 ? !0 : (t = this.onMouse(e), t || (this.targetElement = null), t)
}, FastClick.prototype.destroy = function () {
    "use strict";
    var e = this.layer;
    deviceIsAndroid && (e.removeEventListener("mouseover", this.onMouse, !0), e.removeEventListener("mousedown", this.onMouse, !0), e.removeEventListener("mouseup", this.onMouse, !0)), e.removeEventListener("click", this.onClick, !0), e.removeEventListener("touchstart", this.onTouchStart, !1), e.removeEventListener("touchmove", this.onTouchMove, !1), e.removeEventListener("touchend", this.onTouchEnd, !1), e.removeEventListener("touchcancel", this.onTouchCancel, !1)
}, FastClick.notNeeded = function (e) {
    "use strict";
    var t, n;
    if (typeof window.ontouchstart == "undefined") return !0;
    n = +(/Chrome\/([0-9]+)/.exec(navigator.userAgent) || [, 0])[1];
    if (n) {
        if (!deviceIsAndroid) return !0;
        t = document.querySelector("meta[name=viewport]");
        if (t) {
            if (t.content.indexOf("user-scalable=no") !== -1) return !0;
            if (n > 31 && document.documentElement.scrollWidth <= window.outerWidth) return !0
        }
    }
    return e.style.msTouchAction === "none" ? !0 : !1
}, FastClick.attach = function (e, t) {
    "use strict";
    return new FastClick(e, t)
}, typeof define != "undefined" && define.amd ? define(function () {
    "use strict";
    return FastClick
}) : typeof module != "undefined" && module.exports ? (module.exports = FastClick.attach, module.exports.FastClick = FastClick) : window.FastClick = FastClick, ! function (e) {
    var t, n, r = {
        numOfCol: 5,
        offsetX: 5,
        offsetY: 5,
        blockElement: "div"
    },
        i = [];
    Array.prototype.indexOf || (Array.prototype.indexOf = function (e) {
        var t = this.length >>> 0,
            n = Number(arguments[1]) || 0;
        for (n = 0 > n ? Math.ceil(n) : Math.floor(n), 0 > n && (n += t) ; t > n; n++)
            if (n in this && this[n] === e) return n;
        return -1
    });
    var s = function () {
        i = [];
        for (var e = 0; e < r.numOfCol; e++) o("empty-" + e, e, 0, 1, -r.offsetY)
    },
        o = function (e, t, n, s, o) {
            for (var u = 0; s > u; u++) {
                var a = new Object;
                a.x = t + u, a.size = s, a.endY = n + o + 2 * r.offsetY, i.push(a)
            }
        },
        u = function (e, t) {
            for (var n = 0; t > n; n++) {
                var r = a(e + n, "x");
                i.splice(r, 1)
            }
        },
        a = function (e, t) {
            for (var n = 0; n < i.length; n++) {
                var r = i[n];
                if ("x" == t && r.x == e) return n;
                if ("endY" == t && r.endY == e) return n
            }
        },
        f = function (e, t) {
            for (var n = [], r = 0; t > r; r++) n.push(i[a(e + r, "x")].endY);
            var s = Math.min.apply(Math, n),
                o = Math.max.apply(Math, n);
            return [s, o, n.indexOf(s)]
        },
        l = function (e) {
            if (e > 1) {
                for (var t, n, r = i.length - e, s = !1, o = 0; o < i.length; o++) {
                    var u = i[o],
                        a = u.x;
                    if (a >= 0 && r >= a) {
                        var l = f(a, e);
                        s ? l[1] < t[1] && (t = l, n = a) : (s = !0, t = l, n = a)
                    }
                }
                return [n, t[1]]
            }
            return t = f(0, i.length), [t[2], t[0]]
        },
        c = function (e) {
            !e.data("size") || e.data("size") < 0 ? e.data("size", 1) : e.data("size") > r.numOfCol && e.data("size", r.numOfCol);
            var t = l(e.data("size")),
                i = Math.round(n * e.data("size"));
            e.css({
                width: i - 2 * r.offsetX,
                left: Math.round(t[0] * n),
                top: t[1],
                position: "absolute"
            });
            var s = e.outerHeight();
            u(t[0], e.data("size")), o(e.attr("id"), t[0], t[1], e.data("size"), s)
        };
    e.fn.BlocksIt = function (o) {
        o && "object" == typeof o && e.extend(r, o), t = e(this), n = t.width() / r.numOfCol, s(), t.children(r.blockElement).each(function (t) {
            c(e(this), t)
        });
        var u = f(0, i.length);
        return t.height(u[1] + r.offsetY), this
    }
}(jQuery),
    function (e) {
        function l() {
            var e = "!@#$%^&*()+=[]\\';,/{}|\":<>?~`.-_";
            return e += " ", e
        }

        function c() {
            var e = "Â¬â‚¬Â£Â¦";
            return e
        }

        function h(t, n, r) {
            t.each(function () {
                var t = e(this);
                t.bind("keyup change paste", function (e) {
                    var i = "";
                    e.originalEvent && e.originalEvent.clipboardData && e.originalEvent.clipboardData.getData && (i = e.originalEvent.clipboardData.getData("text/plain")), setTimeout(function () {
                        m(t, n, r, i)
                    }, 0)
                }), t.bind("keypress", function (e) {
                    var i = e.charCode ? e.charCode : e.which;
                    if (v(i) || e.ctrlKey || e.metaKey) return;
                    var s = String.fromCharCode(i),
                        o = t.selection(),
                        u = o.start,
                        a = o.end,
                        f = t.val(),
                        l = f.substring(0, u) + s + f.substring(a),
                        c = n(l, r);
                    c != l && e.preventDefault()
                })
            })
        }

        function p(t, n) {
            var r = parseFloat(e(t).val()),
                i = e(t);
            if (isNaN(r)) {
                i.val("");
                return
            }
            d(n.min) && r < n.min && i.val(""), d(n.max) && r > n.max && i.val("")
        }

        function d(e) {
            return !isNaN(e)
        }

        function v(e) {
            return e >= 32 ? !1 : e == 10 ? !1 : e == 13 ? !1 : !0
        }

        function m(e, t, n, r) {
            var i = e.val();
            i == "" && r.length > 0 && (i = r);
            var s = t(i, n);
            if (i == s) return;
            var o = e.alphanum_caret();
            e.val(s), i.length == s.length + 1 ? e.alphanum_caret(o - 1) : e.alphanum_caret(o)
        }

        function g(n, i) {
            typeof i == "undefined" && (i = t);
            var s, o = {};
            return typeof n == "string" ? s = r[n] : typeof n == "undefined" ? s = {} : s = n, e.extend(o, i, s), typeof o.blacklist == "undefined" && (o.blacklistSet = P(o.allow, o.disallow)), o
        }

        function y(t) {
            var r, s = {};
            return typeof t == "string" ? r = i[t] : typeof t == "undefined" ? r = {} : r = t, e.extend(s, n, r), s
        }

        function b(e, t, n) {
            return n.maxLength && e.length >= n.maxLength ? !1 : n.allow.indexOf(t) >= 0 ? !0 : n.allowSpace && t == " " ? !0 : n.blacklistSet.contains(t) ? !1 : !n.allowNumeric && a[t] ? !1 : !n.allowUpper && M(t) ? !1 : !n.allowLower && _(t) ? !1 : !n.allowCaseless && D(t) ? !1 : !n.allowLatin && f.contains(t) ? !1 : n.allowOtherCharSets ? !0 : a[t] || f.contains(t) ? !0 : !1
        }

        function w(e, t, n) {
            if (a[t]) return S(e, n) ? !1 : T(e, n) ? !1 : x(e, n) ? !1 : N(e + t, n) ? !1 : C(e + t, n) ? !1 : !0;
            if (n.allowPlus && t == "+" && e == "") return !0;
            if (n.allowMinus && t == "-" && e == "") return !0;
            if (t == o && n.allowThouSep && j(e, t)) return !0;
            if (t == u) {
                if (e.indexOf(u) >= 0) return !1;
                if (n.allowDecSep) return !0
            }
            return !1
        }

        function E(e) {
            return e += "", e.replace(/[^0-9]/g, "").length
        }

        function S(e, t) {
            var n = t.maxDigits;
            if (n == "" || isNaN(n)) return !1;
            var r = E(e);
            return r >= n ? !0 : !1
        }

        function x(e, t) {
            var n = t.maxDecimalPlaces;
            if (n == "" || isNaN(n)) return !1;
            var r = e.indexOf(u);
            if (r == -1) return !1;
            var i = e.substring(r),
                s = E(i);
            return s >= n ? !0 : !1
        }

        function T(e, t) {
            var n = t.maxPreDecimalPlaces;
            if (n == "" || isNaN(n)) return !1;
            var r = e.indexOf(u);
            if (r >= 0) return !1;
            var i = E(e);
            return i >= n ? !0 : !1
        }

        function N(e, t) {
            if (!t.max || t.max < 0) return !1;
            var n = parseFloat(e);
            return n > t.max ? !0 : !1
        }

        function C(e, t) {
            if (!t.min || t.min > 0) return !1;
            var n = parseFloat(e);
            return n < t.min ? !0 : !1
        }

        function k(e, t) {
            if (typeof e != "string") return e;
            var n = e.split(""),
                r = [],
                i = 0,
                s;
            for (i = 0; i < n.length; i++) {
                s = n[i];
                var o = r.join("");
                b(o, s, t) && r.push(s)
            }
            var u = r.join("");
            return t.forceLower ? u = u.toLowerCase() : t.forceUpper && (u = u.toUpperCase()), u
        }

        function L(e, t) {
            if (typeof e != "string") return e;
            var n = e.split(""),
                r = [],
                i = 0,
                s;
            for (i = 0; i < n.length; i++) {
                s = n[i];
                var o = r.join("");
                w(o, s, t) && r.push(s)
            }
            return r.join("")
        }

        function A(e) {
            var t = e.split(""),
                n = 0,
                r = [],
                i;
            for (n = 0; n < t.length; n++) i = t[n]
        }

        function O(e) { }

        function M(e) {
            var t = e.toUpperCase(),
                n = e.toLowerCase();
            return e == t && t != n ? !0 : !1
        }

        function _(e) {
            var t = e.toUpperCase(),
                n = e.toLowerCase();
            return e == n && t != n ? !0 : !1
        }

        function D(e) {
            return e.toUpperCase() == e.toLowerCase() ? !0 : !1
        }

        function P(e, t) {
            var n = new F(s + t),
                r = new F(e),
                i = n.subtract(r);
            return i
        }

        function H() {
            var e = "0123456789".split(""),
                t = {},
                n = 0,
                r;
            for (n = 0; n < e.length; n++) r = e[n], t[r] = !0;
            return t
        }

        function B() {
            var e = "abcdefghijklmnopqrstuvwxyz",
                t = e.toUpperCase(),
                n = new F(e + t);
            return n
        }

        function j(e, t) {
            if (e.length == 0) return !1;
            var n = e.indexOf(u);
            if (n >= 0) return !1;
            var r = e.indexOf(o);
            if (r < 0) return !0;
            var i = e.lastIndexOf(o),
                s = e.length - i - 1;
            if (s < 3) return !1;
            var a = E(e.substring(r));
            return a % 3 > 0 ? !1 : !0
        }

        function F(e) {
            typeof e == "string" ? this.map = I(e) : this.map = {}
        }

        function I(e) {
            var t = {},
                n = e.split(""),
                r = 0,
                i;
            for (r = 0; r < n.length; r++) i = n[r], t[i] = !0;
            return t
        }
        e.fn.alphanum = function (e) {
            var t = g(e),
                n = this;
            return h(n, k, t), this
        }, e.fn.alpha = function (e) {
            var t = g("alpha"),
                n = g(e, t),
                r = this;
            return h(r, k, n), this
        }, e.fn.numeric = function (e) {
            var t = y(e),
                n = this;
            return h(n, L, t), n.blur(function () {
                p(this, e)
            }), this
        };
        var t = {
            allow: "",
            disallow: "",
            allowSpace: !0,
            allowNumeric: !0,
            allowUpper: !0,
            allowLower: !0,
            allowCaseless: !0,
            allowLatin: !0,
            allowOtherCharSets: !0,
            forceUpper: !1,
            forceLower: !1,
            maxLength: NaN
        },
            n = {
                allowPlus: !1,
                allowMinus: !0,
                allowThouSep: !0,
                allowDecSep: !0,
                allowLeadingSpaces: !1,
                maxDigits: NaN,
                maxDecimalPlaces: NaN,
                maxPreDecimalPlaces: NaN,
                max: NaN,
                min: NaN
            },
            r = {
                alpha: {
                    allowNumeric: !1
                },
                upper: {
                    allowNumeric: !1,
                    allowUpper: !0,
                    allowLower: !1,
                    allowCaseless: !0
                },
                lower: {
                    allowNumeric: !1,
                    allowUpper: !1,
                    allowLower: !0,
                    allowCaseless: !0
                }
            },
            i = {
                integer: {
                    allowPlus: !1,
                    allowMinus: !0,
                    allowThouSep: !1,
                    allowDecSep: !1
                },
                positiveInteger: {
                    allowPlus: !1,
                    allowMinus: !1,
                    allowThouSep: !1,
                    allowDecSep: !1
                }
            },
            s = l() + c(),
            o = ",",
            u = ".",
            a = H(),
            f = B();
        F.prototype.add = function (e) {
            var t = this.clone();
            for (var n in e.map) t.map[n] = !0;
            return t
        }, F.prototype.subtract = function (e) {
            var t = this.clone();
            for (var n in e.map) delete t.map[n];
            return t
        }, F.prototype.contains = function (e) {
            return this.map[e] ? !0 : !1
        }, F.prototype.clone = function () {
            var e = new F;
            for (var t in this.map) e.map[t] = !0;
            return e
        }, e.fn.alphanum.backdoorAlphaNum = function (e, t) {
            var n = g(t);
            return k(e, n)
        }, e.fn.alphanum.backdoorNumeric = function (e, t) {
            var n = y(t);
            return L(e, n)
        }, e.fn.alphanum.setNumericSeparators = function (e) {
            if (e.thousandsSeparator.length != 1) return;
            if (e.decimalSeparator.length != 1) return;
            o = e.thousandsSeparator, u = e.decimalSeparator
        }
    }(jQuery),
    function (e) {
        function t(e, t) {
            if (e.createTextRange) {
                var n = e.createTextRange();
                n.move("character", t), n.select()
            } else e.selectionStart != null && (e.focus(), e.setSelectionRange(t, t))
        }

        function n(e) {
            if ("selection" in document) {
                var t = e.createTextRange();
                try {
                    t.setEndPoint("EndToStart", document.selection.createRange())
                } catch (n) {
                    return 0
                }
                return t.text.length
            }
            if (e.selectionStart != null) return e.selectionStart
        }
        e.fn.alphanum_caret = function (r, i) {
            return typeof r == "undefined" ? n(this.get(0)) : this.queue(function (n) {
                if (isNaN(r)) {
                    var s = e(this).val().indexOf(r);
                    i === !0 ? s += r.length : typeof i != "undefined" && (s += i), t(this, s)
                } else t(this, r);
                n()
            })
        }
    }(jQuery),
    function (e) {
        var t = function (e) {
            return e.replace(/([a-z])([a-z]+)/gi, function (e, t, n) {
                return t + n.toLowerCase()
            }).replace(/_/g, "")
        },
            n = function (e) {
                return e.replace(/^([a-z]+)_TO_([a-z]+)/i, function (e, t, n) {
                    return n + "_TO_" + t
                })
            },
            r = function (e) {
                return e ? e.ownerDocument.defaultView || e.ownerDocument.parentWindow : window
            },
            i = function (t, n) {
                var r = e.Range.current(t).clone(),
                    i = e.Range(t).select(t);
                return r.overlaps(i) ? (r.compare("START_TO_START", i) < 1 ? (startPos = 0, r.move("START_TO_START", i)) : (fromElementToCurrent = i.clone(), fromElementToCurrent.move("END_TO_START", r), startPos = fromElementToCurrent.toString().length), r.compare("END_TO_END", i) >= 0 ? endPos = i.toString().length : endPos = startPos + r.toString().length, {
                    start: startPos,
                    end: endPos
                }) : null
            },
            s = function (t) {
                var n = r(t);
                if (t.selectionStart !== undefined) return document.activeElement && document.activeElement != t && t.selectionStart == t.selectionEnd && t.selectionStart == 0 ? {
                    start: t.value.length,
                    end: t.value.length
                } : {
                    start: t.selectionStart,
                    end: t.selectionEnd
                };
                if (n.getSelection) return i(t, n);
                try {
                    if (t.nodeName.toLowerCase() == "input") {
                        var s = r(t).document.selection.createRange(),
                            o = t.createTextRange();
                        o.setEndPoint("EndToStart", s);
                        var u = o.text.length;
                        return {
                            start: u,
                            end: u + s.text.length
                        }
                    }
                    var a = i(t, n);
                    if (!a) return a;
                    var f = e.Range.current().clone(),
                        l = f.clone().collapse().range,
                        c = f.clone().collapse(!1).range;
                    return l.moveStart("character", -1), c.moveStart("character", -1), a.startPos != 0 && l.text == "" && (a.startPos += 2), a.endPos != 0 && c.text == "" && (a.endPos += 2), a
                } catch (h) {
                    return {
                        start: t.value.length,
                        end: t.value.length
                    }
                }
            },
            o = function (e, t, n) {
                var i = r(e);
                if (e.setSelectionRange) n === undefined ? (e.focus(), e.setSelectionRange(t, t)) : (e.select(), e.selectionStart = t, e.selectionEnd = n);
                else if (e.createTextRange) {
                    var s = e.createTextRange();
                    s.moveStart("character", t), n = n || t, s.moveEnd("character", n - e.value.length), s.select()
                } else if (i.getSelection) {
                    var o = i.document,
                        u = i.getSelection(),
                        f = o.createRange(),
                        l = [t, n !== undefined ? n : t];
                    a([e], l), f.setStart(l[0].el, l[0].count), f.setEnd(l[1].el, l[1].count), u.removeAllRanges(), u.addRange(f)
                } else if (i.document.body.createTextRange) {
                    var f = document.body.createTextRange();
                    f.moveToElementText(e), f.collapse(), f.moveStart("character", t), f.moveEnd("character", n !== undefined ? n : t), f.select()
                }
            },
            u = function (e, t, n, r) {
                typeof n[0] == "number" && n[0] < t && (n[0] = {
                    el: r,
                    count: n[0] - e
                }), typeof n[1] == "number" && n[1] <= t && (n[1] = {
                    el: r,
                    count: n[1] - e
                })
            },
            a = function (e, t, n) {
                var r, i;
                n = n || 0;
                for (var s = 0; e[s]; s++) r = e[s], r.nodeType === 3 || r.nodeType === 4 ? (i = n, n += r.nodeValue.length, u(i, n, t, r)) : r.nodeType !== 8 && (n = a(r.childNodes, t, n));
                return n
            };
        jQuery.fn.selection = function (e, t) {
            return e !== undefined ? this.each(function () {
                o(this, e, t)
            }) : s(this[0])
        }, e.fn.selection.getCharElement = a
    }(jQuery),
    function (e) {
        var t, n = 0,
            r = 0;
        lob && lob == "flights" && (r = 1);
        var i = {
            isMsie: function () {
                return /(msie|trident)/i.test(navigator.userAgent) ? navigator.userAgent.match(/(msie |rv:)(\d+(.\d+)?)/i)[2] : !1
            },
            isBlankString: function (e) {
                return !e || /^\s*$/.test(e)
            },
            escapeRegExChars: function (e) {
                return e.replace(/[\-\[\]\/\{\}\(\)\*\+\?\.\\\^\$\|]/g, "\\$&")
            },
            isString: function (e) {
                return typeof e == "string"
            },
            isNumber: function (e) {
                return typeof e == "number"
            },
            isArray: e.isArray,
            isFunction: e.isFunction,
            isObject: e.isPlainObject,
            isUndefined: function (e) {
                return typeof e == "undefined"
            },
            bind: e.proxy,
            each: function (t, n) {
                function r(e, t) {
                    return n(t, e)
                }
                e.each(t, r)
            },
            map: e.map,
            filter: e.grep,
            every: function (t, n) {
                var r = !0;
                return t ? (e.each(t, function (e, i) {
                    if (!(r = n.call(null, i, e, t))) return !1
                }), !!r) : r
            },
            some: function (t, n) {
                var r = !1;
                return t ? (e.each(t, function (e, i) {
                    if (r = n.call(null, i, e, t)) return !1
                }), !!r) : r
            },
            mixin: e.extend,
            getUniqueId: function () {
                var e = 0;
                return function () {
                    return e++
                }
            }(),
            templatify: function (n) {
                function r() {
                    return String(n)
                }
                return e.isFunction(n) ? n : r
            },
            defer: function (e) {
                setTimeout(e, 0)
            },
            debounce: function (e, t, n) {
                var r, i;
                return function () {
                    var s = this,
                        o = arguments,
                        u, a;
                    return u = function () {
                        r = null, n || (i = e.apply(s, o))
                    }, a = n && !r, clearTimeout(r), r = setTimeout(u, t), a && (i = e.apply(s, o)), i
                }
            },
            throttle: function (e, t) {
                var n, r, i, s, o, u;
                return o = 0, u = function () {
                    o = new Date, i = null, s = e.apply(n, r)
                },
                    function () {
                        var a = new Date,
                            f = t - (a - o);
                        return n = this, r = arguments, f <= 0 ? (clearTimeout(i), i = null, o = a, s = e.apply(n, r)) : i || (i = setTimeout(u, f)), s
                    }
            },
            noop: function () { }
        },
            s = "0.10.2",
            o = function (e) {
                function t(e) {
                    return e.split(/\s+/)
                }

                function n(e) {
                    return e.split(/\W+/)
                }

                function r(e) {
                    return function (n) {
                        return function (r) {
                            return e(r[n])
                        }
                    }
                }
                return {
                    nonword: n,
                    whitespace: t,
                    obj: {
                        nonword: r(n),
                        whitespace: r(t)
                    }
                }
            }(),
            u = function () {
                function e(e) {
                    this.maxSize = e || 100, this.size = 0, this.hash = {}, this.list = new t
                }

                function t() {
                    this.head = this.tail = null
                }

                function n(e, t) {
                    this.key = e, this.val = t, this.prev = this.next = null
                }
                return i.mixin(e.prototype, {
                    set: function (t, r) {
                        var i = this.list.tail,
                            s;
                        this.size >= this.maxSize && (this.list.remove(i), delete this.hash[i.key]), (s = this.hash[t]) ? (s.val = r, this.list.moveToFront(s)) : (s = new n(t, r), this.list.add(s), this.hash[t] = s, this.size++)
                    },
                    get: function (t) {
                        var n = this.hash[t];
                        if (n) return this.list.moveToFront(n), n.val
                    }
                }), i.mixin(t.prototype, {
                    add: function (t) {
                        this.head && (t.next = this.head, this.head.prev = t), this.head = t, this.tail = this.tail || t
                    },
                    remove: function (t) {
                        t.prev ? t.prev.next = t.next : this.head = t.next, t.next ? t.next.prev = t.prev : this.tail = t.prev
                    },
                    moveToFront: function (e) {
                        this.remove(e), this.add(e)
                    }
                }), e
            }(),
            a = function () {
                function r(e) {
                    this.prefix = ["__", e, "__"].join(""), this.ttlKey = "__ttl__", this.keyMatcher = new RegExp("^" + this.prefix)
                }

                function s() {
                    return (new Date).getTime()
                }

                function o(e) {
                    return JSON.stringify(i.isUndefined(e) ? null : e)
                }

                function u(e) {
                    return JSON.parse(e)
                }
                var e, t;
                try {
                    e = window.localStorage, e.setItem("~~~", "!"), e.removeItem("~~~")
                } catch (n) {
                    e = null
                }
                return e && window.JSON ? t = {
                    _prefix: function (e) {
                        return this.prefix + e
                    },
                    _ttlKey: function (e) {
                        return this._prefix(e) + this.ttlKey
                    },
                    get: function (t) {
                        return this.isExpired(t) && this.remove(t), u(e.getItem(this._prefix(t)))
                    },
                    set: function (t, n, r) {
                        return i.isNumber(r) ? e.setItem(this._ttlKey(t), o(s() + r)) : e.removeItem(this._ttlKey(t)), e.setItem(this._prefix(t), o(n))
                    },
                    remove: function (t) {
                        return e.removeItem(this._ttlKey(t)), e.removeItem(this._prefix(t)), this
                    },
                    clear: function () {
                        var t, n, r = [],
                            i = e.length;
                        for (t = 0; t < i; t++) (n = e.key(t)).match(this.keyMatcher) && r.push(n.replace(this.keyMatcher, ""));
                        for (t = r.length; t--;) this.remove(r[t]);
                        return this
                    },
                    isExpired: function (t) {
                        var n = u(e.getItem(this._ttlKey(t)));
                        return i.isNumber(n) && s() > n ? !0 : !1
                    }
                } : t = {
                    get: i.noop,
                    set: i.noop,
                    remove: i.noop,
                    clear: i.noop,
                    isExpired: i.noop
                }, i.mixin(r.prototype, t), r
            }(),
            f = function () {
                function o(t) {
                    t = t || {}, this._send = t.transport ? a(t.transport) : e.ajax, this._get = t.rateLimiter ? t.rateLimiter(this._get) : this._get
                }

                function a(t) {
                    return function (r, s) {
                        function u(e) {
                            i.defer(function () {
                                o.resolve(e)
                            })
                        }

                        function a(e) {
                            i.defer(function () {
                                o.reject(e)
                            })
                        }
                        var o = e.Deferred();
                        return t(r, s, u, a), o
                    }
                }
                var t = 0,
                    n = {},
                    r = 6,
                    s = new u(10);
                return o.setMaxPendingRequests = function (t) {
                    r = t
                }, o.resetCache = function () {
                    s = new u(10)
                }, i.mixin(o.prototype, {
                    _get: function (e, i, o) {
                        function f(t) {
                            o && o(null, t), s.set(e, t)
                        }

                        function l() {
                            o && o(!0)
                        }

                        function c() {
                            t--, delete n[e], u.onDeckRequestArgs && (u._get.apply(u, u.onDeckRequestArgs), u.onDeckRequestArgs = null)
                        }
                        var u = this,
                            a;
                        (a = n[e]) ? a.done(f).fail(l) : t < r ? (t++, n[e] = this._send(e, i).done(f).fail(l).always(c)) : this.onDeckRequestArgs = [].slice.call(arguments, 0)
                    },
                    get: function (e, t, n) {
                        var r;
                        return i.isFunction(t) && (n = t, t = {}), (r = s.get(e)) ? i.defer(function () {
                            n && n(null, r)
                        }) : this._get(e, t, n), !!r
                    }
                }), o
            }(),
            l = function () {
                function t(t) {
                    t = t || {}, (!t.datumTokenizer || !t.queryTokenizer) && e.error("datumTokenizer and queryTokenizer are both required"), this.datumTokenizer = t.datumTokenizer, this.queryTokenizer = t.queryTokenizer, this.reset()
                }

                function n(e) {
                    return e = i.filter(e, function (e) {
                        return !!e
                    }), e = i.map(e, function (e) {
                        return e.toLowerCase()
                    }), e
                }

                function r() {
                    return {
                        ids: [],
                        children: {}
                    }
                }

                function s(e) {
                    var t = {},
                        n = [];
                    for (var r = 0; r < e.length; r++) t[e[r]] || (t[e[r]] = !0, n.push(e[r]));
                    return n
                }

                function o(e, t) {
                    function s(e, t) {
                        return e - t
                    }
                    var n = 0,
                        r = 0,
                        i = [];
                    e = e.sort(s), t = t.sort(s);
                    while (n < e.length && r < t.length) e[n] < t[r] ? n++ : e[n] > t[r] ? r++ : (i.push(e[n]), n++, r++);
                    return i
                }
                return i.mixin(t.prototype, {
                    bootstrap: function (t) {
                        this.datums = t.datums, this.trie = t.trie
                    },
                    add: function (e) {
                        var t = this;
                        e = i.isArray(e) ? e : [e], i.each(e, function (e) {
                            var s, o;
                            s = t.datums.push(e) - 1, o = n(t.datumTokenizer(e)), i.each(o, function (e) {
                                var n, i, o;
                                n = t.trie, i = e.split("");
                                while (o = i.shift()) n = n.children[o] || (n.children[o] = r()), n.ids.push(s)
                            })
                        })
                    },
                    get: function (t) {
                        var r = this,
                            u, a;
                        return u = n(this.queryTokenizer(t)), i.each(u, function (e) {
                            var t, n, i, s;
                            if (a && a.length === 0) return !1;
                            t = r.trie, n = e.split("");
                            while (t && (i = n.shift())) t = t.children[i];
                            if (!t || n.length !== 0) return a = [], !1;
                            s = t.ids.slice(0), a = a ? o(a, s) : s
                        }), a ? i.map(s(a), function (e) {
                            return r.datums[e]
                        }) : []
                    },
                    reset: function () {
                        this.datums = [], this.trie = r()
                    },
                    serialize: function () {
                        return {
                            datums: this.datums,
                            trie: this.trie
                        }
                    }
                }), t
            }(),
            c = function () {
                function t(e) {
                    return e.local || null
                }

                function n(t) {
                    var n, r;
                    r = {
                        url: null,
                        thumbprint: "",
                        ttl: 864e5,
                        filter: null,
                        ajax: {}
                    };
                    if (n = t.prefetch || null) n = i.isString(n) ? {
                        url: n
                    } : n, n = i.mixin(r, n), n.thumbprint = s + n.thumbprint, n.ajax.type = n.ajax.type || "GET", n.ajax.dataType = n.ajax.dataType || "json", !n.url && e.error("prefetch requires url to be set");
                    return n
                }

                function r(t) {
                    function s(e) {
                        return function (t) {
                            return i.debounce(t, e)
                        }
                    }

                    function o(e) {
                        return function (t) {
                            return i.throttle(t, e)
                        }
                    }
                    var n, r;
                    r = {
                        url: null,
                        wildcard: "%QUERY",
                        replace: null,
                        rateLimitBy: "debounce",
                        rateLimitWait: 300,
                        send: null,
                        filter: null,
                        ajax: {}
                    };
                    if (n = t.remote || null) n = i.isString(n) ? {
                        url: n
                    } : n, n = i.mixin(r, n), n.rateLimiter = /^throttle$/i.test(n.rateLimitBy) ? o(n.rateLimitWait) : s(n.rateLimitWait), n.ajax.type = n.ajax.type || "GET", n.ajax.dataType = n.ajax.dataType || "json", delete n.rateLimitBy, delete n.rateLimitWait, !n.url && e.error("remote requires url to be set");
                    return n
                }
                return {
                    local: t,
                    prefetch: n,
                    remote: r
                }
            }();
        (function (t) {
            function s(t) {
                (!t || !t.local && !t.prefetch && !t.remote) && e.error("one of local, prefetch, or remote is required"), this.limit = t.limit || 5, this.sorter = u(t.sorter), this.dupDetector = t.dupDetector || h, this.local = c.local(t), this.prefetch = c.prefetch(t), this.remote = c.remote(t), this.cacheKey = this.prefetch ? this.prefetch.cacheKey || this.prefetch.url : null, this.index = new l({
                    datumTokenizer: t.datumTokenizer,
                    queryTokenizer: t.queryTokenizer
                }), this.storage = this.cacheKey ? new a(this.cacheKey) : null
            }

            function u(e) {
                function t(t) {
                    return t.sort(e)
                }

                function n(e) {
                    return e
                }
                return i.isFunction(e) ? t : n
            }

            function h() {
                return !1
            }
            var n, r;
            return n = t.Bloodhound, r = {
                data: "data",
                protocol: "protocol",
                thumbprint: "thumbprint"
            }, t.Bloodhound = s, s.noConflict = function () {
                return t.Bloodhound = n, s
            }, s.tokenizers = o, i.mixin(s.prototype, {
                _loadPrefetch: function (n) {
                    function o(e) {
                        r.clear(), r.add(n.filter ? n.filter(e) : e), r._saveToStorage(r.index.serialize(), n.thumbprint, n.ttl)
                    }
                    var r = this,
                        i, s;
                    return (i = this._readFromStorage(n.thumbprint)) ? (this.index.bootstrap(i), s = e.Deferred().resolve()) : s = e.ajax(n.url, n.ajax).done(o), s
                },
                _getFromRemote: function (t, n) {
                    function o(e, t) {
                        e ? n([]) : n(r.remote.filter ? r.remote.filter(t) : t)
                    }
                    var r = this,
                        i, s;
                    return t = t || "", s = encodeURIComponent(t), i = this.remote.replace ? this.remote.replace(this.remote.url, t) : this.remote.url.replace(this.remote.wildcard, s), this.transport.get(i, this.remote.ajax, o)
                },
                _saveToStorage: function (t, n, i) {
                    this.storage && (this.storage.set(r.data, t, i), this.storage.set(r.protocol, location.protocol, i), this.storage.set(r.thumbprint, n, i))
                },
                _readFromStorage: function (t) {
                    var n = {},
                        i;
                    return this.storage && (n.data = this.storage.get(r.data), n.protocol = this.storage.get(r.protocol), n.thumbprint = this.storage.get(r.thumbprint)), i = n.thumbprint !== t || n.protocol !== location.protocol, n.data && !i ? n.data : null
                },
                _initialize: function () {
                    function o() {
                        n.add(i.isFunction(r) ? r() : r)
                    }
                    var n = this,
                        r = this.local,
                        s;
                    return s = this.prefetch ? this._loadPrefetch(this.prefetch) : e.Deferred().resolve(), r && s.done(o), this.transport = this.remote ? new f(this.remote) : null, this.initPromise = s.promise()
                },
                initialize: function (t) {
                    return !this.initPromise || t ? this._initialize() : this.initPromise
                },
                add: function (t) {
                    this.index.add(t)
                },
                get: function (t, n) {
                    function u(e) {
                        var t = s.slice(0);
                        i.each(e, function (e) {
                            var n;
                            return n = i.some(t, function (t) {
                                return r.dupDetector(e, t)
                            }), !n && t.push(e), t.length < r.limit
                        }), n && n(r.sorter(t))
                    }
                    var r = this,
                        s = [],
                        o = !1;
                    s = this.index.get(t), s = this.sorter(s).slice(0, this.limit), s.length < this.limit && this.transport && (o = this._getFromRemote(t, u)), o || (s.length > 0 || !this.transport) && n && n(s)
                },
                clear: function () {
                    this.index.reset()
                },
                clearPrefetchCache: function () {
                    this.storage && this.storage.clear()
                },
                clearRemoteCache: function () {
                    this.transport && f.resetCache()
                },
                ttAdapter: function () {
                    return i.bind(this.get, this)
                }
            }), s
        })(this);
        var h = {
            wrapper: '<span class="twitter-typeahead"></span>',
            dropdown: '<span class="tt-dropdown-menu"></span>',
            dataset: '<div class="tt-dataset-%CLASS%"></div>',
            suggestions: '<span class="tt-suggestions"></span>',
            suggestion: '<div class="tt-suggestion"></div>'
        },
            p = {
                wrapper: {
                    position: "relative",
                    display: "inline-block"
                },
                hint: {
                    position: "absolute",
                    top: "0",
                    left: "0",
                    borderColor: "transparent",
                    boxShadow: "none"
                },
                input: {
                    position: "relative",
                    verticalAlign: "top",
                    backgroundColor: "transparent"
                },
                inputWithNoHint: {
                    position: "relative",
                    verticalAlign: "top"
                },
                dropdown: {
                    position: "absolute",
                    top: "100%",
                    left: "0",
                    zIndex: "100",
                    display: "none"
                },
                suggestions: {
                    display: "block"
                },
                suggestion: {
                    whiteSpace: "nowrap",
                    cursor: "pointer"
                },
                suggestionChild: {
                    whiteSpace: "normal"
                },
                ltr: {
                    left: "0",
                    right: "auto"
                },
                rtl: {
                    left: "auto",
                    right: " 0"
                }
            };
        i.isMsie() && i.mixin(p.input, {
            backgroundImage: "url(data:image/gif;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7)"
        }), i.isMsie() && i.isMsie() <= 7 && i.mixin(p.input, {
            marginTop: "-1px"
        });
        var d = function () {
            function n(t) {
                (!t || !t.el) && e.error("EventBus initialized without el"), this.$el = e(t.el)
            }
            var t = "typeahead:";
            return i.mixin(n.prototype, {
                trigger: function (e) {
                    var n = [].slice.call(arguments, 1);
                    this.$el.trigger(t + e, n)
                }
            }), n
        }(),
            v = function () {
                function n(t, n, r, i) {
                    var s;
                    if (!r) return this;
                    n = n.split(e), r = i ? f(r, i) : r, this._callbacks = this._callbacks || {};
                    while (s = n.shift()) this._callbacks[s] = this._callbacks[s] || {
                        sync: [],
                        async: []
                    }, this._callbacks[s][t].push(r);
                    return this
                }

                function r(e, t, r) {
                    return n.call(this, "async", e, t, r)
                }

                function i(e, t, r) {
                    return n.call(this, "sync", e, t, r)
                }

                function s(t) {
                    var n;
                    if (!this._callbacks) return this;
                    t = t.split(e);
                    while (n = t.shift()) delete this._callbacks[n];
                    return this
                }

                function o(n) {
                    var r, i, s, o, a;
                    if (!this._callbacks) return this;
                    n = n.split(e), s = [].slice.call(arguments, 1);
                    while ((r = n.shift()) && (i = this._callbacks[r])) o = u(i.sync, this, [r].concat(s)), a = u(i.async, this, [r].concat(s)), o() && t(a);
                    return this
                }

                function u(e, t, n) {
                    function r() {
                        var r;
                        for (var i = 0; !r && i < e.length; i += 1) r = e[i].apply(t, n) === !1;
                        return !r
                    }
                    return r
                }

                function a() {
                    var e;
                    return window.setImmediate ? e = function (t) {
                        setImmediate(function () {
                            t()
                        })
                    } : e = function (t) {
                        setTimeout(function () {
                            t()
                        }, 0)
                    }, e
                }

                function f(e, t) {
                    return e.bind ? e.bind(t) : function () {
                        e.apply(t, [].slice.call(arguments, 0))
                    }
                }
                var e = /\s+/,
                    t = a();
                return {
                    onSync: i,
                    onAsync: r,
                    off: s,
                    trigger: o
                }
            }(),
            m = function (e) {
                function n(e, t, n) {
                    var r = [],
                        s;
                    for (var o = 0; o < e.length; o++) r.push(i.escapeRegExChars(e[o]));
                    return s = n ? "\\b(" + r.join("|") + ")\\b" : "(" + r.join("|") + ")", t ? new RegExp(s) : new RegExp(s, "i")
                }
                var t = {
                    node: null,
                    pattern: null,
                    tagName: "strong",
                    className: null,
                    wordsOnly: !1,
                    caseSensitive: !1
                };
                return function (s) {
                    function u(t) {
                        var n, r;
                        if (n = o.exec(t.data)) wrapperNode = e.createElement(s.tagName), s.className && (wrapperNode.className = s.className), r = t.splitText(n.index), r.splitText(n[0].length), wrapperNode.appendChild(r.cloneNode(!0)), t.parentNode.replaceChild(wrapperNode, r);
                        return !!n
                    }

                    function a(e, t) {
                        var n, r = 3;
                        for (var i = 0; i < e.childNodes.length; i++) n = e.childNodes[i], n.nodeType === r ? i += t(n) ? 1 : 0 : a(n, t)
                    }
                    var o;
                    s = i.mixin({}, t, s);
                    if (!s.node || !s.pattern) return;
                    s.pattern = i.isArray(s.pattern) ? s.pattern : [s.pattern], o = n(s.pattern, s.caseSensitive, s.wordsOnly), a(s.node, u)
                }
            }(window.document),
            g = function () {
                function n(n) {
                    var s = this,
                        o, u, a, f;
                    n = n || {}, n.input || e.error("input is missing"), o = i.bind(this._onBlur, this), u = i.bind(this._onFocus, this), a = i.bind(this._onKeydown, this), f = i.bind(this._onInput, this), this.$hint = e(n.hint), this.$input = e(n.input).on("blur.tt", o).on("focus.tt", u).on("keydown.tt", a), this.$hint.length === 0 && (this.setHint = this.getHint = this.clearHint = this.clearHintIfInvalid = i.noop), i.isMsie() ? this.$input.on("keydown.tt keypress.tt cut.tt paste.tt", function (e) {
                        if (t[e.which || e.keyCode]) return;
                        i.defer(i.bind(s._onInput, s, e))
                    }) : this.$input.on("input.tt", f), this.query = this.$input.val(), this.$overflowHelper = r(this.$input)
                }

                function r(t) {
                    return e('<pre aria-hidden="true"></pre>').css({
                        position: "absolute",
                        visibility: "hidden",
                        whiteSpace: "pre",
                        fontFamily: t.css("font-family"),
                        fontSize: t.css("font-size"),
                        fontStyle: t.css("font-style"),
                        fontVariant: t.css("font-variant"),
                        fontWeight: t.css("font-weight"),
                        wordSpacing: t.css("word-spacing"),
                        letterSpacing: t.css("letter-spacing"),
                        textIndent: t.css("text-indent"),
                        textRendering: t.css("text-rendering"),
                        textTransform: t.css("text-transform")
                    }).insertAfter(t)
                }

                function s(e, t) {
                    return n.normalizeQuery(e) === n.normalizeQuery(t)
                }

                function o(e) {
                    return e.altKey || e.ctrlKey || e.metaKey || e.shiftKey
                }
                var t;
                return t = {
                    9: "tab",
                    27: "esc",
                    37: "left",
                    39: "right",
                    13: "enter",
                    38: "up",
                    40: "down"
                }, n.normalizeQuery = function (e) {
                    return (e || "").replace(/^\s*/g, "").replace(/\s{2,}/g, " ")
                }, i.mixin(n.prototype, v, {
                    _onBlur: function () {
                        this.resetInputValue(), this.trigger("blurred")
                    },
                    _onFocus: function () {
                        this.trigger("focused")
                    },
                    _onKeydown: function (n) {
                        var r = t[n.which || n.keyCode];
                        this._managePreventDefault(r, n), r && this._shouldTrigger(r, n) && this.trigger(r + "Keyed", n)
                    },
                    _onInput: function () {
                        this._checkInputValue()
                    },
                    _managePreventDefault: function (t, n) {
                        var r, i, s;
                        switch (t) {
                            case "tab":
                                i = this.getHint(), s = this.getInputValue(), r = i && i !== s && !o(n);
                                break;
                            case "up":
                            case "down":
                                r = !o(n);
                                break;
                            default:
                                r = !1
                        }
                        r && n.preventDefault()
                    },
                    _shouldTrigger: function (t, n) {
                        var r;
                        switch (t) {
                            case "tab":
                                r = !o(n);
                                break;
                            default:
                                r = !0
                        }
                        return r
                    },
                    _checkInputValue: function () {
                        var t, n, r;
                        t = this.getInputValue(), n = s(t, this.query), r = n ? this.query.length !== t.length : !1, n ? r && this.trigger("whitespaceChanged", this.query) : this.trigger("queryChanged", this.query = t)
                    },
                    focus: function () {
                        this.$input.focus()
                    },
                    blur: function () {
                        this.$input.blur()
                    },
                    getQuery: function () {
                        return this.query
                    },
                    setQuery: function (t) {
                        this.query = t
                    },
                    getInputValue: function () {
                        return this.$input.val()
                    },
                    setInputValue: function (t, n) {
                        if (t.indexOf("<span") > -1 || t.indexOf("<>") > -1) t = t.substring(0, t.indexOf("<"));
                        this.$input.val(t), n ? this.clearHint() : this._checkInputValue()
                    },
                    resetInputValue: function () {
                        this.setInputValue(this.query, !0)
                    },
                    getHint: function () {
                        return this.$hint.val()
                    },
                    setHint: function (t) {
                        this.$hint.val(t)
                    },
                    clearHint: function () {
                        this.setHint("")
                    },
                    clearHintIfInvalid: function () {
                        var t, n, r, i;
                        t = this.getInputValue(), n = this.getHint(), r = t !== n && n.indexOf(t) === 0, i = t !== "" && r && !this.hasOverflow(), !i && this.clearHint()
                    },
                    getLanguageDirection: function () {
                        return (this.$input.css("direction") || "ltr").toLowerCase()
                    },
                    hasOverflow: function () {
                        var t = this.$input.width() - 2;
                        return this.$overflowHelper.text(this.getInputValue()), this.$overflowHelper.width() >= t
                    },
                    isCursorAtEnd: function () {
                        var e, t, n;
                        return e = this.$input.val().length, t = this.$input[0].selectionStart, i.isNumber(t) ? t === e : document.selection ? (n = document.selection.createRange(), n.moveStart("character", -e), e === n.text.length) : !0
                    },
                    destroy: function () {
                        this.$hint.off(".tt"), this.$input.off(".tt"), this.$hint = this.$input = this.$overflowHelper = null
                    }
                }), n
            }(),
            y = function () {
                function a(t) {
                    t = t || {}, t.templates = t.templates || {}, t.source || e.error("missing source"), t.name && !c(t.name) && e.error("invalid dataset name: " + t.name), this.query = null, this.highlight = !!t.highlight, this.name = t.name || i.getUniqueId(), this.source = t.source, this.displayFn = f(t.display || t.displayKey), this.templates = l(t.templates, this.displayFn), this.$el = e(h.dataset.replace("%CLASS%", this.name))
                }

                function f(e) {
                    function t(t) {
                        return t[e]
                    }
                    return e = e || "value", i.isFunction(e) ? e : t
                }

                function l(e, t) {
                    function n(e) {
                        return "<p>" + t(e) + "</p>"
                    }
                    return {
                        empty: e.empty && i.templatify(e.empty),
                        header: e.header && i.templatify(e.header),
                        footer: e.footer && i.templatify(e.footer),
                        suggestion: e.suggestion || n
                    }
                }

                function c(e) {
                    return /^[_a-zA-Z0-9-]+$/.test(e)
                }
                var s = "ttDataset",
                    o = "ttValue",
                    u = "ttDatum";
                return a.extractDatasetName = function (n) {
                    return e(n).data(s)
                }, a.extractValue = function (n) {
                    return e(n).data(o)
                }, a.extractDatum = function (n) {
                    return e(n).data(u)
                }, i.mixin(a.prototype, v, {
                    _render: function (f, l) {
                        function v() {
                            return t && clearTimeout(t), t = setTimeout(function () {
                                try {
                                    if (lob && lob == "flights") {
                                        var e = s_gi("mmtprod");
                                        e.linkTrackVars = "prop24,prop54,eVar15,eVar24", e.prop54 = "No_Results_Found-" + f, e.tl(this, "o", "No_Results_Found-" + f)
                                    }
                                } catch (t) { }
                            }, 2e3), c.templates.empty({
                                query: f,
                                isEmpty: !0
                            })
                        }

                        function g() {
                            function a(t) {
                                var n;
                                return n = e(h.suggestion).append(c.templates.suggestion(t)).data(s, c.name).data(o, c.displayFn(t)).data(u, t), n.children().each(function () {
                                    e(this).css(p.suggestionChild)
                                }), n
                            }
                            t && clearTimeout(t);
                            var n, r;
                            return n = e(h.suggestions).css(p.suggestions), r = i.map(l, a), n.append.apply(n, r), c.highlight && m({
                                node: n[0],
                                pattern: f
                            }), n
                        }

                        function y() {
                            return c.templates.header({
                                query: f,
                                isEmpty: !d
                            })
                        }

                        function b() {
                            return c.templates.footer({
                                query: f,
                                isEmpty: !d
                            })
                        }
                        if (!this.$el) return;
                        var c = this,
                            d;
                        this.$el.empty(), d = l && l.length, !d && this.templates.empty ? r ? n && this.$el.html(v()).prepend(c.templates.header ? y() : null).append(c.templates.footer ? b() : null) : this.$el.html(v()).prepend(c.templates.header ? y() : null).append(c.templates.footer ? b() : null) : d && this.$el.html(g()).prepend(c.templates.header ? y() : null).append(c.templates.footer ? b() : null), n = !d, this.trigger("rendered")
                    },
                    getRoot: function () {
                        return this.$el
                    },
                    update: function (t) {
                        function r(e) {
                            !n.canceled && t === n.query && n._render(t, e)
                        }
                        var n = this;
                        this.query = t, this.canceled = !1, this.source(t, r)
                    },
                    cancel: function () {
                        this.canceled = !0
                    },
                    clear: function () {
                        this.cancel(), this.$el.empty(), this.trigger("rendered")
                    },
                    isEmpty: function () {
                        return this.$el.is(":empty")
                    },
                    destroy: function () {
                        this.$el = null
                    }
                }), a
            }(),
            b = function () {
                function t(t) {
                    var n = this,
                        r, o, u;
                    t = t || {}, t.menu || e.error("menu is required"), this.isOpen = !1, this.isEmpty = !0, this.datasets = i.map(t.datasets, s), r = i.bind(this._onSuggestionClick, this), o = i.bind(this._onSuggestionMouseEnter, this), u = i.bind(this._onSuggestionMouseLeave, this), this.$menu = e(t.menu).on("click.tt", ".tt-suggestion", r).on("mouseenter.tt", ".tt-suggestion", o).on("mouseleave.tt", ".tt-suggestion", u), i.each(this.datasets, function (e) {
                        n.$menu.append(e.getRoot()), e.onSync("rendered", n._onRendered, n)
                    })
                }

                function s(e) {
                    return new y(e)
                }
                return i.mixin(t.prototype, v, {
                    _onSuggestionClick: function (n) {
                        this.trigger("suggestionClicked", e(n.currentTarget))
                    },
                    _onSuggestionMouseEnter: function (n) {
                        this._removeCursor(), this._setCursor(e(n.currentTarget), !0)
                    },
                    _onSuggestionMouseLeave: function () {
                        this._removeCursor()
                    },
                    _onRendered: function () {
                        function t(e) {
                            return e.isEmpty()
                        }
                        this.isEmpty = i.every(this.datasets, t), this.isEmpty ? this._hide() : this.isOpen && this._show(), this.trigger("datasetRendered")
                    },
                    _hide: function () {
                        this.$menu.hide()
                    },
                    _show: function () {
                        this.$menu.css("display", "block")
                    },
                    _getSuggestions: function () {
                        return this.$menu.find(".tt-suggestion")
                    },
                    _getCursor: function () {
                        return this.$menu.find(".tt-cursor").first()
                    },
                    _setCursor: function (t, n) {
                        t.first().addClass("tt-cursor"), !n && this.trigger("cursorMoved")
                    },
                    _removeCursor: function () {
                        this._getCursor().removeClass("tt-cursor")
                    },
                    _moveCursor: function (t) {
                        var n, r, i, s;
                        if (!this.isOpen) return;
                        r = this._getCursor(), n = this._getSuggestions(), this._removeCursor(), i = n.index(r) + t, i = (i + 1) % (n.length + 1) - 1;
                        if (i === -1) {
                            this.trigger("cursorRemoved");
                            return
                        }
                        i < -1 && (i = n.length - 1), this._setCursor(s = n.eq(i)), this._ensureVisible(s)
                    },
                    _ensureVisible: function (t) {
                        var n, r, i, s;
                        n = t.position().top, r = n + t.outerHeight(!0), i = this.$menu.scrollTop(), s = this.$menu.height() + parseInt(this.$menu.css("paddingTop"), 10) + parseInt(this.$menu.css("paddingBottom"), 10), n < 0 ? this.$menu.scrollTop(i + n) : s < r && this.$menu.scrollTop(i + (r - s))
                    },
                    close: function () {
                        this.isOpen && (this.isOpen = !1, this._removeCursor(), this._hide(), this.trigger("closed"))
                    },
                    open: function () {
                        this.isOpen || (this.isOpen = !0, !this.isEmpty && this._show(), this.trigger("opened"))
                    },
                    setLanguageDirection: function (t) {
                        this.$menu.css(t === "ltr" ? p.ltr : p.rtl)
                    },
                    moveCursorUp: function () {
                        this._moveCursor(-1), this.getDatumForCursor() !== null && this.getDatumForCursor().raw.select === !1 && this._moveCursor(-1)
                    },
                    moveCursorDown: function () {
                        this._moveCursor(1), this.getDatumForCursor() !== null && this.getDatumForCursor().raw.select === !1 && this._moveCursor(1)
                    },
                    getDatumForSuggestion: function (t) {
                        var n = null;
                        return t.length && (n = {
                            raw: y.extractDatum(t),
                            value: y.extractValue(t),
                            datasetName: y.extractDatasetName(t)
                        }), n
                    },
                    getDatumForCursor: function () {
                        return this.getDatumForSuggestion(this._getCursor().first())
                    },
                    getDatumForTopSuggestion: function () {
                        return this.getDatumForSuggestion(this._getSuggestions().first())
                    },
                    update: function (t) {
                        function s(e) {
                            e.update(t)
                        }
                        r && t == "" && (!IS_MOBILE || !!IS_TAB) && (t = "<*>"), n = 0, i.each(this.datasets, s)
                    },
                    empty: function () {
                        function t(e) {
                            e.clear()
                        }
                        i.each(this.datasets, t), this.isEmpty = !0
                    },
                    isVisible: function () {
                        return this.isOpen && !this.isEmpty
                    },
                    destroy: function () {
                        function t(e) {
                            e.destroy()
                        }
                        this.$menu.off(".tt"), this.$menu = null, i.each(this.datasets, t)
                    }
                }), t
            }(),
            w = function () {
                function n(t) {
                    var n, r, o;
                    t = t || {}, t.input || e.error("missing input"), this.isActivated = !1, this.autoselect = !!t.autoselect, this.minLength = i.isNumber(t.minLength) ? t.minLength : 1, this.$node = s(t.input, t.withHint), n = this.$node.find(".tt-dropdown-menu"), r = this.$node.find(".tt-input"), o = this.$node.find(".tt-hint"), r.on("blur.tt", function (e) {
                        var t, s, o;
                        t = document.activeElement, s = n.is(t), o = n.has(t).length > 0, i.isMsie() && (s || o) && (e.preventDefault(), e.stopImmediatePropagation(), i.defer(function () {
                            r.focus()
                        }))
                    }), n.on("mousedown.tt", function (e) {
                        e.preventDefault()
                    }), this.eventBus = t.eventBus || new d({
                        el: r
                    }), this.dropdown = (new b({
                        menu: n,
                        datasets: t.datasets
                    })).onSync("suggestionClicked", this._onSuggestionClicked, this).onSync("cursorMoved", this._onCursorMoved, this).onSync("cursorRemoved", this._onCursorRemoved, this).onSync("opened", this._onOpened, this).onSync("closed", this._onClosed, this).onAsync("datasetRendered", this._onDatasetRendered, this), this.input = (new g({
                        input: r,
                        hint: o
                    })).onSync("focused", this._onFocused, this).onSync("blurred", this._onBlurred, this).onSync("enterKeyed", this._onEnterKeyed, this).onSync("tabKeyed", this._onTabKeyed, this).onSync("escKeyed", this._onEscKeyed, this).onSync("upKeyed", this._onUpKeyed, this).onSync("downKeyed", this._onDownKeyed, this).onSync("leftKeyed", this._onLeftKeyed, this).onSync("rightKeyed", this._onRightKeyed, this).onSync("queryChanged", this._onQueryChanged, this).onSync("whitespaceChanged", this._onWhitespaceChanged, this), this._setLanguageDirection()
                }

                function s(n, r) {
                    var i, s, u, a;
                    i = e(n), s = e(h.wrapper).css(p.wrapper), u = e(h.dropdown).css(p.dropdown), a = i.clone().css(p.hint).css(o(i)), a.val("").removeData().addClass("tt-hint").removeAttr("id name placeholder").prop("disabled", !0).attr({
                        autocomplete: "off",
                        spellcheck: "false"
                    }), i.data(t, {
                        dir: i.attr("dir"),
                        autocomplete: i.attr("autocomplete"),
                        spellcheck: i.attr("spellcheck"),
                        style: i.attr("style")
                    }), i.addClass("tt-input").attr({
                        autocomplete: "off",
                        spellcheck: !1
                    }).css(r ? p.input : p.inputWithNoHint);
                    try {
                        !i.attr("dir") && i.attr("dir", "auto")
                    } catch (f) { }
                    return i.wrap(s).parent().prepend(r ? a : null).append(u)
                }

                function o(e) {
                    return {
                        backgroundAttachment: e.css("background-attachment"),
                        backgroundClip: e.css("background-clip"),
                        backgroundColor: e.css("background-color"),
                        backgroundImage: e.css("background-image"),
                        backgroundOrigin: e.css("background-origin"),
                        backgroundPosition: e.css("background-position"),
                        backgroundRepeat: e.css("background-repeat"),
                        backgroundSize: e.css("background-size")
                    }
                }

                function u(e) {
                    var n = e.find(".tt-input");
                    i.each(n.data(t), function (e, t) {
                        i.isUndefined(e) ? n.removeAttr(t) : n.attr(t, e)
                    }), n.detach().removeData(t).removeClass("tt-input").insertAfter(e), e.remove()
                }
                var t = "ttAttrs";
                return i.mixin(n.prototype, {
                    _onSuggestionClicked: function (t, n) {
                        var r;
                        (r = this.dropdown.getDatumForSuggestion(n)) && r.raw.select === !0 && this._select(r)
                    },
                    _onCursorMoved: function () {
                        var t = this.dropdown.getDatumForCursor();
                        if (t.value.indexOf("<span") > -1 || t.value.indexOf("<>") > -1) t.value = t.value.substring(0, t.value.indexOf("<span"));
                        this.input.setInputValue(t.value, !0), this.eventBus.trigger("cursorchanged", t.raw, t.datasetName)
                    },
                    _onCursorRemoved: function () {
                        this.input.resetInputValue(), this._updateHint()
                    },
                    _onDatasetRendered: function () {
                        this._updateHint()
                    },
                    _onOpened: function () {
                        this._updateHint(), this.eventBus.trigger("opened")
                    },
                    _onClosed: function () {
                        this.input.clearHint(), this.eventBus.trigger("closed")
                    },
                    _onFocused: function () {
                        this.isActivated = !0;
                        if (r) {
                            var t = this.input.getInputValue(),
                                n = g.normalizeQuery(t);
                            n == "" && (!IS_MOBILE || !!IS_TAB) && this.dropdown.update("<*>")
                        }
                        this.dropdown.open()
                    },
                    _onBlurred: function () {
                        this._autocomplete(!0), this.isActivated = !1, this.dropdown.close()
                    },
                    _onEnterKeyed: function (t, n) {
                        var r, i;
                        r = this.dropdown.getDatumForCursor(), i = this.dropdown.getDatumForTopSuggestion(), r && r.raw.select === !1 ? (this.dropdown._moveCursor(1), r = this.dropdown.getDatumForCursor()) : i && i.raw.select === !1 && (this.dropdown._moveCursor(2), i = this.dropdown.getDatumForCursor()), r ? (this._select(r), n.preventDefault()) : this.autoselect && i && (this._select(i), n.preventDefault())
                    },
                    _onTabKeyed: function (t, n) {
                        var r;
                        (r = this.dropdown.getDatumForCursor()) ? (r.raw.select !== !1 ? this._select(r) : r.raw.select === !1 && (this.dropdown._moveCursor(1), r = this.dropdown.getDatumForCursor(), this._select(r)), n.preventDefault()) : this._autocomplete(!0)
                    },
                    _onEscKeyed: function () {
                        this.dropdown.close()
                    },
                    _onUpKeyed: function () {
                        var t = this.input.getQuery();
                        this.dropdown.isEmpty && t.length >= this.minLength ? this.dropdown.update(t) : this.dropdown.moveCursorUp(), this.dropdown.open()
                    },
                    _onDownKeyed: function () {
                        var t = this.input.getQuery();
                        this.dropdown.isEmpty && t.length >= this.minLength ? this.dropdown.update(t) : this.dropdown.moveCursorDown(), this.dropdown.open()
                    },
                    _onLeftKeyed: function () { },
                    _onRightKeyed: function () { },
                    _onQueryChanged: function (t, n) {
                        this.input.clearHintIfInvalid(), n.length >= this.minLength ? this.dropdown.update(n) : this.dropdown.empty(), this.dropdown.open(), this._setLanguageDirection()
                    },
                    _onWhitespaceChanged: function () {
                        this._updateHint(), this.dropdown.open()
                    },
                    _setLanguageDirection: function () {
                        var t;
                        this.dir !== (t = this.input.getLanguageDirection()) && (this.dir = t, this.$node.css("direction", t), this.dropdown.setLanguageDirection(t))
                    },
                    _updateHint: function () {
                        var t, n, r, s, o, u;
                        t = this.dropdown.getDatumForTopSuggestion(), t && this.dropdown.isVisible() && !this.input.hasOverflow() ? (n = this.input.getInputValue(), r = g.normalizeQuery(n), s = i.escapeRegExChars(r), o = new RegExp("^(?:" + s + ")(.+$)", "i"), u = o.exec(t.value), u ? this.input.setHint(n + u[1]) : this.input.clearHint()) : this.input.clearHint()
                    },
                    _autocomplete: function (t) {
                        var n, r, i, s;
                        n = this.input.getHint(), r = this.input.getQuery(), i = t || this.input.isCursorAtEnd(), n && r !== n && i ? (s = this.dropdown.getDatumForTopSuggestion(), s && this.input.setInputValue(s.value), this.eventBus.trigger("autocompleted", s.raw, s.datasetName)) : !n && r !== n && i && (this.dropdown._moveCursor(1), s = this.dropdown.getDatumForCursor(), s && s.raw.select === !1 && this.dropdown._moveCursor(1), s = this.dropdown.getDatumForCursor(), s && this._select(s))
                    },
                    _select: function (t) {
                        this.input.setQuery(t.value), this.input.setInputValue(t.value, !0), this._setLanguageDirection(), this.eventBus.trigger("selected", t.raw, t.datasetName), this.dropdown.close(), i.defer(i.bind(this.dropdown.empty, this.dropdown))
                    },
                    open: function () {
                        this.dropdown.open()
                    },
                    close: function () {
                        this.dropdown.close()
                    },
                    setVal: function (t) {
                        this.isActivated ? this.input.setInputValue(t) : (this.input.setQuery(t), this.input.setInputValue(t, !0)), this._setLanguageDirection()
                    },
                    getVal: function () {
                        return this.input.getQuery()
                    },
                    destroy: function () {
                        this.input.destroy(), this.dropdown.destroy(), u(this.$node), this.$node = null
                    }
                }), n
            }();
        (function () {
            var t, n, r;
            t = e.fn.typeahead, n = "ttTypeahead", r = {
                initialize: function (r, s) {
                    function o() {
                        var t = e(this),
                            o, u;
                        i.each(s, function (e) {
                            e.highlight = !!r.highlight
                        }), u = new w({
                            input: t,
                            eventBus: o = new d({
                                el: t
                            }),
                            withHint: i.isUndefined(r.hint) ? !0 : !!r.hint,
                            minLength: r.minLength,
                            autoselect: r.autoselect,
                            datasets: s
                        }), t.data(n, u)
                    }
                    return s = i.isArray(s) ? s : [].slice.call(arguments, 1), r = r || {}, this.each(o)
                },
                open: function () {
                    function r() {
                        var t = e(this),
                            r;
                        (r = t.data(n)) && r.open()
                    }
                    return this.each(r)
                },
                close: function () {
                    function r() {
                        var t = e(this),
                            r;
                        (r = t.data(n)) && r.close()
                    }
                    return this.each(r)
                },
                val: function (r) {
                    function i() {
                        var t = e(this),
                            i;
                        (i = t.data(n)) && i.setVal(r)
                    }

                    function s(e) {
                        var t, r;
                        if (t = e.data(n)) r = t.getVal();
                        return r
                    }
                    return arguments.length ? this.each(i) : s(this.first())
                },
                destroy: function () {
                    function r() {
                        var t = e(this),
                            r;
                        if (r = t.data(n)) r.destroy(), t.removeData(n)
                    }
                    return this.each(r)
                }
            }, e.fn.typeahead = function (e) {
                return r[e] ? r[e].apply(this, [].slice.call(arguments, 1)) : r.initialize.apply(this, arguments)
            }, e.fn.typeahead.noConflict = function () {
                return e.fn.typeahead = t, this
            }
        })()
    }(window.jQuery), decrementer = function (e, t, n) {
        var r = $("." + e).html(),
            i = r;
        r = parseInt(r, 10), r > t && r--, $("." + e).html(r);
        if (n == "hotel" || n == "bus") {
            var s = e.split("_");
            $("#ageDiv_" + s[1] + "_" + i).hide()
        }
    }, incrementer = function (e, t, n) {
        var r = $("." + e).html();
        r = parseInt(r, 10), r < t && r++, $("." + e).html(r);
        if (n == "hotel" || n == "bus") {
            var i = e.split("_");
            $("#ageDiv_" + i[1] + "_" + r).show();
            var s = parseInt($("#ex_" + i[1] + "_" + r).data("slider").getValue());
            s == 1 && $("#ageDiv_" + i[1] + "_" + r + " div.tooltip.top").css("left", "-20.5px")
        }
    }, change_class_trip_type = function (e, t) {
        $("." + t).removeClass("active"), $("." + e).addClass("active");
        var n = e.toLowerCase() === "mobile_economy" ? "E" : "B";
        $("#class_selector").val(n), $(".class_type").attr("value", n)
    }, updateDateTexts = function (e) {
        var t = e,
            n = e.attr("value").split("/")[0],
            r = e.attr("value").split("/")[1],
            i = e.attr("value").split("/")[2],
            s = $.datepicker.formatDate("y", t.datepicker("getDate"));
        e.parent().children("span.date").html(r), e.parent().children("span.month_day").children("span.day").html(i), e.parent().children("span.month_day").children("span.month_year").html(n + " '" + s)
    }, document.onclick = function (e) {
        window.customEvent = e || window.event
    }, $(window).on("popstate", function () {
        setTimeout(function () {
            $(".btn-lg").removeClass("loading")
        }, 0)
    }), mmt.nhp.submitToLanding = function (e, t, n, r, i) {
        setTimeout(function () {
            window.customEvent.target.className += " loading"
        }, 0);
        var s = $.extend({}, t),
            o = $("<form></form>");
        o.attr("action", e), o.attr("target", i ? i : ""), o.css("display", "none"), n && o.attr("method", n ? n : "GET");
        for (var u in s) {
            var a = $("<input type='hidden' />");
            a.attr("name", u), a.attr("value", s[u]), o.append(a)
        }
        $("body").append(o);
        var f = mmt.nhp;
        if (r && lob) try {
            localStorage.setItem(lob, JSON.stringify(r))
        } catch (l) { }
        o.submit()
    }, mmt.flightstatuspp = function () {
        newwindow = window.open("http://www.flightstats.com/go/weblet?guid=c228b59beca1b817:1fb62a4f:11502c4a893:6b48&imageColor=black&language=English&weblet=status&action=display", "flightstatus", "scrollbars=yes,height=444,width=500")
    }, $("#mobile_from_typeahead").alphanum({
        allow: "@-',",
        disallow: "",
        allowSpace: !0,
        allowNumeric: !1,
        allowUpper: !0,
        allowLower: !0,
        allowCaseless: !0,
        allowLatin: !0,
        allowOtherCharSets: !0,
        forceUpper: !1,
        forceLower: !1,
        maxLength: NaN
    }), $("#mobile_to_typeahead").alphanum({
        allow: "@-',",
        disallow: "",
        allowSpace: !0,
        allowNumeric: !1,
        allowUpper: !0,
        allowLower: !0,
        allowCaseless: !0,
        allowLatin: !0,
        allowOtherCharSets: !0,
        forceUpper: !1,
        forceLower: !1,
        maxLength: NaN
    }),
    function (e) {
        e.fn.slides = function (t) {
            return t = e.extend({}, e.fn.slides.option, t), this.each(function () {
                function S(o, u, a) {
                    if (!v && d) {
                        v = !0, t.animationStart(p + 1);
                        switch (o) {
                            case "next":
                                c = p, l = p + 1, l = i === l ? 0 : l, g = s * 2, o = -s * 2, p = l;
                                break;
                            case "prev":
                                c = p, l = p - 1, l = l === -1 ? i - 1 : l, g = 0, o = 0, p = l;
                                break;
                            case "pagination":
                                l = parseInt(a, 10), c = e("." + t.paginationClass + " li." + t.currentClass + " a", n).attr("href").match("[^#/]+$"), l > c ? (g = s * 2, o = -s * 2) : (g = 0, o = 0), p = l
                        }
                        u === "fade" ? t.crossfade ? r.children(":eq(" + l + ")", n).css({
                            zIndex: 10
                        }).fadeIn(t.fadeSpeed, t.fadeEasing, function () {
                            t.autoHeight ? r.animate({
                                height: r.children(":eq(" + l + ")", n).outerHeight()
                            }, t.autoHeightSpeed, function () {
                                r.children(":eq(" + c + ")", n).css({
                                    display: "none",
                                    zIndex: 0
                                }), r.children(":eq(" + l + ")", n).css({
                                    zIndex: 0
                                }), t.animationComplete(l + 1), v = !1
                            }) : (r.children(":eq(" + c + ")", n).css({
                                display: "none",
                                zIndex: 0
                            }), r.children(":eq(" + l + ")", n).css({
                                zIndex: 0
                            }), t.animationComplete(l + 1), v = !1)
                        }) : r.children(":eq(" + c + ")", n).fadeOut(t.fadeSpeed, t.fadeEasing, function () {
                            t.autoHeight ? r.animate({
                                height: r.children(":eq(" + l + ")", n).outerHeight()
                            }, t.autoHeightSpeed, function () {
                                r.children(":eq(" + l + ")", n).fadeIn(t.fadeSpeed, t.fadeEasing)
                            }) : r.children(":eq(" + l + ")", n).fadeIn(t.fadeSpeed, t.fadeEasing, function () {
                                e.browser.msie && e(this).get(0).style.removeAttribute("filter")
                            }), t.animationComplete(l + 1), v = !1
                        }) : (r.children(":eq(" + l + ")").css({
                            left: g,
                            display: "block"
                        }), t.autoHeight ? r.animate({
                            left: o,
                            height: r.children(":eq(" + l + ")").outerHeight()
                        }, t.slideSpeed, t.slideEasing, function () {
                            r.css({
                                left: -s
                            }), r.children(":eq(" + l + ")").css({
                                left: s,
                                zIndex: 0
                            }), r.children(":eq(" + c + ")").css({
                                left: s,
                                display: "none",
                                zIndex: 0
                            }), t.animationComplete(l + 1), v = !1
                        }) : r.animate({
                            left: o
                        }, t.slideSpeed, t.slideEasing, function () {
                            r.css({
                                left: -s
                            }), r.children(":eq(" + l + ")").css({
                                left: s,
                                zIndex: 0
                            }), r.children(":eq(" + c + ")").css({
                                left: s,
                                display: "none",
                                zIndex: 0
                            }), t.animationComplete(l + 1), v = !1
                        })), t.pagination && (e("." + t.paginationClass + " li." + t.currentClass, n).removeClass(t.currentClass), e("." + t.paginationClass + " li:eq(" + l + ")", n).addClass(t.currentClass))
                    }
                }

                function x() {
                    clearInterval(n.data("interval"))
                }

                function T() {
                    t.pause ? (clearTimeout(n.data("pause")), clearInterval(n.data("interval")), w = setTimeout(function () {
                        clearTimeout(n.data("pause")), E = setInterval(function () {
                            S("next", a)
                        }, t.play), n.data("interval", E)
                    }, t.pause), n.data("pause", w)) : x()
                }
                e("." + t.container, e(this)).children().wrapAll('<div class="slides_control"/>');
                var n = e(this),
                    r = e(".slides_control", n),
                    i = r.children().size(),
                    s = r.children().outerWidth(),
                    o = r.children().outerHeight(),
                    u = t.start - 1,
                    a = t.effect.indexOf(",") < 0 ? t.effect : t.effect.replace(" ", "").split(",")[0],
                    f = t.effect.indexOf(",") < 0 ? a : t.effect.replace(" ", "").split(",")[1],
                    l = 0,
                    c = 0,
                    h = 0,
                    p = 0,
                    d, v, m, g, y, b, w, E;
                if (i < 2) return e("." + t.container, e(this)).fadeIn(t.fadeSpeed, t.fadeEasing, function () {
                    d = !0, t.slidesLoaded()
                }), e("." + t.next + ", ." + t.prev).fadeOut(0), !1;
                if (i < 2) return;
                u < 0 && (u = 0), u > i && (u = i - 1), t.start && (p = u), t.randomize && r.randomize(), e("." + t.container, n).css({
                    overflow: "hidden",
                    position: "relative"
                }), r.children().css({
                    position: "absolute",
                    top: 0,
                    left: r.children().outerWidth(),
                    zIndex: 0,
                    display: "none"
                }), r.css({
                    position: "relative",
                    width: s * 3,
                    height: o,
                    left: -s
                }), e("." + t.container, n).css({
                    display: "block"
                }), t.autoHeight && (r.children().css({
                    height: "auto"
                }), r.animate({
                    height: r.children(":eq(" + u + ")").outerHeight()
                }, t.autoHeightSpeed));
                if (t.preload && r.find("img:eq(" + u + ")").length) {
                    e("." + t.container, n).css({
                        background: "url(" + t.preloadImage + ") no-repeat 50% 50%"
                    });
                    var N = r.find("img:eq(" + u + ")").attr("src") + "?" + (new Date).getTime();
                    e("img", n).parent().attr("class") != "slides_control" ? b = r.children(":eq(0)")[0].tagName.toLowerCase() : b = r.find("img:eq(" + u + ")"), r.find("img:eq(" + u + ")").attr("src", N).load(function () {
                        r.find(b + ":eq(" + u + ")").fadeIn(t.fadeSpeed, t.fadeEasing, function () {
                            e(this).css({
                                zIndex: 0
                            }), e("." + t.container, n).css({
                                background: ""
                            }), d = !0, t.slidesLoaded()
                        })
                    })
                } else r.children(":eq(" + u + ")").fadeIn(t.fadeSpeed, t.fadeEasing, function () {
                    d = !0, t.slidesLoaded()
                });
                t.bigTarget && (r.children().css({
                    cursor: "pointer"
                }), r.children().click(function () {
                    return S("next", a), !1
                })), t.hoverPause && t.play && (r.bind("mouseover", function () {
                    x()
                }), r.bind("mouseleave", function () {
                    T()
                })), t.generateNextPrev && (e("." + t.container, n).after('<a href="#" class="' + t.prev + '">Prev</a>'), e("." + t.prev, n).after('<a href="#" class="' + t.next + '">Next</a>')), e("." + t.next, n).click(function (n) {
                    n.preventDefault(), t.play && T(), e(this).hasClass("next_disabled") == 0 && S("next", a)
                }), e("." + t.prev, n).click(function (n) {
                    n.preventDefault(), t.play && T(), e(this).hasClass("prev_disabled") == 0 && S("prev", a)
                }), t.generatePagination ? (t.prependPagination ? n.prepend("<ul class=" + t.paginationClass + "></ul>") : n.append("<ul class=" + t.paginationClass + "></ul>"), r.children().each(function () {
                    e("." + t.paginationClass, n).append('<li><a href="#' + h + '">' + (h + 1) + "</a></li>"), h++
                })) : e("." + t.paginationClass + " li a", n).each(function () {
                    e(this).attr("href", "#" + h), h++
                }), e("." + t.paginationClass + " li:eq(" + u + ")", n).addClass(t.currentClass), e("." + t.paginationClass + " li a", n).click(function () {
                    return t.play && T(), m = e(this).attr("href").match("[^#/]+$"), p != m && S("pagination", f, m), !1
                }), e("a.link", n).click(function () {
                    return t.play && T(), m = e(this).attr("href").match("[^#/]+$") - 1, p != m && S("pagination", f, m), !1
                }), t.play && (E = setInterval(function () {
                    S("next", a)
                }, t.play), n.data("interval", E))
            })
        }, e.fn.slides.option = {
            preload: !1,
            preloadImage: "/img/loading.gif",
            container: "slides_container",
            generateNextPrev: !1,
            next: "next",
            prev: "prev",
            pagination: !0,
            generatePagination: !0,
            prependPagination: !1,
            paginationClass: "pagination",
            currentClass: "current",
            fadeSpeed: 350,
            fadeEasing: "",
            slideSpeed: 350,
            slideEasing: "",
            start: 1,
            effect: "slide",
            crossfade: !1,
            randomize: !1,
            play: 0,
            pause: 0,
            hoverPause: !1,
            autoHeight: !1,
            autoHeightSpeed: 350,
            bigTarget: !1,
            animationStart: function () { },
            animationComplete: function () { },
            slidesLoaded: function () { }
        }, e.fn.randomize = function (t) {
            function n() {
                return Math.round(Math.random()) - .5
            }
            return e(this).each(function () {
                var r = e(this),
                    s = r.children(),
                    o = s.length;
                if (o > 1) {
                    s.hide();
                    var u = [];
                    for (i = 0; i < o; i++) u[u.length] = i;
                    u = u.sort(n), e.each(u, function (e, n) {
                        var i = s.eq(n),
                            o = i.clone(!0);
                        o.show().appendTo(r), t !== undefined && t(i, o), i.remove()
                    })
                }
            })
        }
    }(jQuery), $(document).ready(function () {
        $.type("#loader") != "undefined" && $("#loader").remove(), $.type("div.chf_header div.chf_right_portion_hdr") != "undefined" && $("div.chf_header div.chf_right_portion_hdr").show();
        var e = "ontouchend" in document,
            t = function (e) {
                var t = $(this),
                    n = t.attr("rel"),
                    r = t.parent("li:eq(0)");
                if (t.parent("li").hasClass("active")) {
                    var i = t.attr("href");
                    return window.open(i, "_blank"), !1
                }
                return $("ul.all_menuoptionsList > li > a").not(t).parent("li").removeClass("active"), t.parent("li").addClass("active"), $(".allMenu_data").not($(n)).stop(!1, !0).fadeOut(0), $(n).stop(!1, !0).fadeIn(0), e.preventDefault(), !1
            },
            n = function (e) {
                e.preventDefault();
                var t = $(this);
                $("ul.all_menuoptionsList > li > a").not(t).parent("li").removeClass("active"), t.parent("li").addClass("active");
                var n = t.attr("rel");
                $(".allMenu_data").not($(n)).stop(!1, !0).fadeOut(0), $(n).stop(!1, !0).fadeIn(0)
            };
        e ? navigator.userAgent.match(/iPhone/i) || navigator.userAgent.match(/iPod/i) || navigator.userAgent.match(/iPad/i) ? ($("ul.all_menuoptionsList > li > a").hover(n), $("body").addClass("touch")) : $("ul.all_menuoptionsList > li > a").click(t) : $("ul.all_menuoptionsList > li > a").hover(n);
        var r = function (e) {
            $("a.all_menuitem").addClass("active"), $(".fadeOverlay") && $(".fadeOverlay").remove();
            var t = $("<div class='fadeOverlay'/>");
            t.css({
                height: $(document).height(),
                width: $(document).width(),
                background: "#000000",
                opacity: .8,
                "z-index": 99,
                position: "absolute",
                left: 0,
                top: 0
            }), $("body").append(t), $("div.fadeOverlay").on("touchend click", function () {
                $("a.all_menuitem").removeClass("active"), $(".all_menudropdown").hide(), $("a.all_menuitem").removeClass("active"), $(this).remove(), $("a.all_menuitem").bind("click", r)
            }), $(document).keyup(function (e) {
                e.keyCode == 27 && $(".all_menudropdown").is(":visible") == 1 && i()
            }), $(".all_menudropdown").is(":visible") == 0 && $(".all_menudropdown").show(), $("a.all_menuitem").unbind("click", r)
        };
        $("#cls_btn").click(function () {
            i()
        });
        var i = function () {
            $("a.all_menuitem").removeClass("active"), $(".all_menudropdown").hide(), $("a.all_menuitem").removeClass("active"), $("div.fadeOverlay").remove(), $("a.all_menuitem").bind("click", r)
        };
        $("a.all_menuitem").bind("click", r)
    });
var Base64 = {};
Base64.code = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=", Base64.encode = function (e, t) {
    t = typeof t == "undefined" ? !1 : t;
    var n, r, i, s, o, u, a, f, l = [],
        c = "",
        h, p, d, v = Base64.code;
    p = t ? Utf8.encode(e) : e, h = p.length % 3;
    if (h > 0)
        while (h++ < 3) c += "=", p += "\0";
    for (h = 0; h < p.length; h += 3) n = p.charCodeAt(h), r = p.charCodeAt(h + 1), i = p.charCodeAt(h + 2), s = n << 16 | r << 8 | i, o = s >> 18 & 63, u = s >> 12 & 63, a = s >> 6 & 63, f = s & 63, l[h / 3] = v.charAt(o) + v.charAt(u) + v.charAt(a) + v.charAt(f);
    return d = l.join(""), d = d.slice(0, d.length - c.length) + c, d
}, Base64.decode = function (e, t) {
    t = typeof t == "undefined" ? !1 : t;
    var n, r, i, s, o, u, a, f, l = [],
        c, h, p = Base64.code;
    h = t ? Utf8.decode(e) : e;
    for (var d = 0; d < h.length; d += 4) s = p.indexOf(h.charAt(d)), o = p.indexOf(h.charAt(d + 1)), u = p.indexOf(h.charAt(d + 2)), a = p.indexOf(h.charAt(d + 3)), f = s << 18 | o << 12 | u << 6 | a, n = f >>> 16 & 255, r = f >>> 8 & 255, i = f & 255, l[d / 4] = String.fromCharCode(n, r, i), a == 64 && (l[d / 4] = String.fromCharCode(n, r)), u == 64 && (l[d / 4] = String.fromCharCode(n));
    return c = l.join(""), t ? Utf8.decode(c) : c
}