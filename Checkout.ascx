<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Checkout.ascx.cs" Inherits="Aramark.Widgets.Checkout" %>
<%@ Import Namespace="Aramark.SuiteCatering.SuiteWizardAPI" %>
<script src="/resources/js/jquery.blockUI.js"></script>

<div class="po">
	<div class="po-panes">
		<div class="po-pane-move">
			<div class="po-pane po-cnfrm po-order">
				<h1 class="center">Billing Information.</h1>
				<nav class="step">
					<ul class="step-4">
						<li class="complete">1</li>
						<li class="complete">2</li>
						<li class="complete">3</li>
						<li class="active last">4</li>
					</ul>
				</nav>
				<nav class="utility">
				     <a href="/order/confirmation/" class="btn back"><span>Back</span></a> 
                     <asp:LinkButton runat="server" CssClass="btn" ID="lnkSubmitCreditTop" OnClick="lnkSubmitCredit_Click" style="display: none;"><span>Submit Order</span></asp:LinkButton>
                       <asp:Button ID="btnPostNew" runat="server" Height="0px" Width="0px" Visible="true" OnClientClick="return true;" OnClick="btnPostNew_Click1" />
                     <%--<a href="#" class="btn" id="lnkSubmitCreditTop" runat="server" style="display: none;"><span>Submit Order</span></a> --%>
                     <asp:LinkButton runat="server" CssClass="btn" OnClientClick="return forceCreditCard();blockUI();" id="lnkSubmitTop" OnClick="lnkSubmit_OnClick"><span>Submit Order</span></asp:LinkButton>
                    <asp:LinkButton runat="server" CssClass="btn" id="LinkButton1" OnClick="lnkCancel_OnClick"><span>Cancel</span></asp:LinkButton>
				</nav>
				<div class="billing-form">
					<fieldset class="card-on-file">
						<div class="form-el check">
                            <p runat="server" ID="pCCPlaceholder"></p>
                            <div class="clear"></div>
							<input type="checkbox" id="savedPayment" checked="checked" />
							<label id="lblSavedPayment" for="savedPayment" onclick="toggleNewCard(this);" class="checked">Please <font color="red"><strong >uncheck</strong></font> the box to enter a different payment method. <br /> You will then have the option to change for this order only, or to change the default. </label>
						</div>
					</fieldset>
					
					<fieldset class="new-card" style="display: none;">
                        <div class="form-el" id="convergeError" runat="server" visible="false">
                            <font color="red"><strong>TPlease check your card details and resubmit.</strong></font>
                        </div>
                        <ul>
                        <div class="form-el radio">
                            <input type="radio" id="orderOnly" runat="server" clientidmode="Static" class="cardSave" name="cardSave" checked="True" value="orderOnly" />
                            <label for="orderOnly" class="checked"><span>This order only</span></label>
                            <div class="clear"></div>
                        </div>
                        <div class="form-el radio">
                            <input type="radio" id="futureOrders" runat="server" clientidmode="Static" class="cardSave" name="cardSave" value="futureOrders" />
                            <label for="futureOrders"><span>Save and make default payment</span></label>
                            <div class="clear"></div>
                        </div>
                            </ul>
                        <div class="payment">
							<ul>
								<li class="title">Payment Method</li>
								<li>
									<div class="form-el radio">
										<input type="radio" id="cardVisa" runat="server" clientidmode="Static" class="cardType"  name="cardType" checked="True" value="969f005b-088e-11d7-8e54-00508bca2b32" />
										<label for="cardVisa"><span class="card-visa">Visa</span></label>
										<div class="clear"></div>
									</div>
								</li>

								<li>
									<div class="form-el radio">
										<input type="radio" id="cardMC" runat="server" clientidmode="Static" class="cardType" name="cardType" value="969f0059-088e-11d7-8e54-00508bca2b32" />
										<label for="cardMC"><span class="card-mc">MasterCard</span></label>
										<div class="clear"></div>
									</div>
								</li>
								<li>
									<div class="form-el radio">
										<input type="radio" id="cardAMEX" runat="server" clientidmode="Static" class="cardType"  name="cardType" value="969f005c-088e-11d7-8e54-00508bca2b32" />
										<label for="cardAMEX"><span class="card-amex">American Express</span></label>
										<div class="clear"></div>
									</div>
								</li>
								<li>
									<div class="form-el radio">
										<input type="radio" id="cardDisc" runat="server" clientidmode="Static" class="cardType"  name="cardType" value="969f0062-088e-11d7-8e54-00508bca2b32" />
										<label for="cardDisc"><span class="card-disc">Discover</span></label>
										<div class="clear"></div>
									</div>
								</li>
								<li class="clear"></li>
							</ul>
						</div>
						<%--		<div class="form-el">
							<label for="cardName">*Name on Card</label>
							<input id="cardName" type="text" required="required" />
							<span class="err">Name on card is required</span>
							<div class="clear"></div>
						</div>--%>
                        <div class="form-el col-50">
							<label for="cardFirstName">*First Name</label>
							<input id="cardFirstName" type="text" required="required"  runat="server"/>
							<span class="err">First Name is required</span>
							<div class="clear"></div>
						</div>
                        <div class="form-el col-50 col-last">
							<label for="cardLastName">*Last Name</label>
							<input id="cardLastName" type="text" required="required" runat="server" />
							<span class="err">Last Name is required</span>
							<div class="clear"></div>
						</div>
						<div class="form-el col-60">
							<label for="cardNum">*Credit Card Number</label>
							<input id="cardNum" type="text" required="required" maxlength="16" runat="server"/>
							<span class="err">Credit card number on card is required</span>
							<div class="clear"></div>
						</div>
						<div class="form-el col-40 col-last">
							<label for="cardCSV">*Security Code</label>
							<input id="cardCSV" type="text" required="required"  maxlength="4" runat="server"/>
							<span class="err">Security code is required</span>
							<div class="clear"></div>
						</div>
						<div class="form-el col-25 spacer">
							<label for="cardMonth">*Expiration Date</label>
							<select id="cardMonth" required="required" runat="server">
								<option value="0">Month</option>
								<option value="01">January</option>
								<option value="02">February</option>
								<option value="03">March</option>
								<option value="04">April</option>
								<option value="05">May</option>
								<option value="06">June</option>
								<option value="07">July</option>
								<option value="08">August</option>
								<option value="09">September</option>
								<option value="10">October</option>
								<option value="11">November</option>
								<option value="12">December</option> 
							</select>
                            <span class="err">Invalid Month</span>
							<div class="clear"></div>
						</div>
						<div class="form-el col-25 no-label">
							<asp:DropDownList runat="server" ID="cardYear"/>
							<div class="clear"></div>
						</div>
						<div class="clear"></div>
					</fieldset>
				</div>
				<div class="order-info menu-info">
					<h2>Your Order</h2>
					<p class="suite-info"><strong>Location:</strong> <asp:Literal runat="server" ID="litLocation"></asp:Literal><br />
                        <% 
                        String currentHost = HttpContext.Current.Request.Url.Host.ToLower();
                        if(currentHost.Contains("boothcatering")) { %>
						<strong>Booth:</strong> <% } else { %> <strong>Suite:</strong> <% } %> <asp:Literal runat="server" ID="litSuite"></asp:Literal><br />
						<strong>Date:</strong> <asp:Literal runat="server" ID="litDate"></asp:Literal><br />
						<strong>Event:</strong> <asp:Literal runat="server" ID="litEvent"></asp:Literal><br />
						<%  if(currentHost.Contains("boothcatering")) { %> <strong>Onsite Contact:</strong>  <% } else { %> <strong>Hosted by:</strong> <% } %>  <asp:Literal runat="server" ID="litEmail"></asp:Literal><br />
						<%  if(currentHost.Contains("suitecatering")) { %><strong>Account Options:</strong> <asp:Literal runat="server" ID="litOptions"></asp:Literal><br /> <% } %>
						<%  if(currentHost.Contains("boothcatering")) { %> <strong>Booth #:</strong>  <% } else { %> <strong>VIP Guests:</strong> <% } %> <asp:Literal runat="server" ID="litVIP"></asp:Literal></p>
					<h2>Menu</h2>
					<table>
						<tbody>
                            <asp:Repeater runat="server" ID="rptPackages" OnItemDataBound="rptPackageItems_OnItemDataBound">
						        <ItemTemplate>
						            <tr id="mainRow" runat="server" class="package-head">
						                <td class="large first"><%# Eval("Title") %></td>
								        <td class="x-large">Qty: <%# Eval("Quantity") %></td>
								        <td class="small last">$<%# Eval("TotalPrice") %></td>
							        </tr>
							        <asp:Repeater runat="server" ID="rptPackageItems">
							            <HeaderTemplate>
							                <tr class="package-cnt">
								                <td colspan="3">
								                    <ul>
							            </HeaderTemplate>
							            <ItemTemplate>
							                <li><%# Eval("PackageMenuItemTitle") %><%# GetDeliveryTime((OrderDetailPackageItem)Container.DataItem) %></li>
										</ItemTemplate>
                                        <FooterTemplate>
                                                    </ul>
								                </td>
							                </tr>      
                                        </FooterTemplate>
							        </asp:Repeater>
						        </ItemTemplate>
						    </asp:Repeater>
                            <asp:Repeater runat="server" ID="rptAlaCarte">
							    <ItemTemplate>
							        <tr>
							        	<td class="large first"><%# Eval("Title") %><%# GetDeliveryTime((OrderDetail)Container.DataItem) %></td>
								        <td class="x-large">Qty: <%# Eval("Quantity") %></td>
								        <td class="small last">$<%# Eval("TotalPrice") %></td>
							        </tr>        
							    </ItemTemplate>
							</asp:Repeater>
						</tbody>
					</table>
					<ul class="calculation">
						<li><span class="line-item">Subtotal</span><span class="amount">$<asp:Literal runat="server" ID="phSubTotal"></asp:Literal></span></li>
						<asp:Repeater runat="server" ID="rptFeeds">
						    <ItemTemplate>
						        <li><span class="line-item"><%# Eval("Description") %></span><span class="amount">$<%# Eval("Amount") %></span></li>        
						    </ItemTemplate>
						</asp:Repeater>
						<li class="total"> <span class="line-item">Total</span><span class="amount">$<asp:Literal runat="server" ID="phGrandTotal"></asp:Literal></span>
							<div class="clear"></div>
						</li>
					</ul>
					<div class="clear"></div>
					<p class="disclaimer"><asp:Literal runat="server" ID="litDisclaimer"></asp:Literal></p>
				</div>
				<div class="cmmnt">
					<h2>Your Comments/Special Instructions</h2>
					<p><asp:Literal runat="server" ID="litComments"></asp:Literal></p>
				</div>
				<nav class="utility">
				    <a href="/order/confirmation/" class="btn back"><span>Back</span></a> 
                   <asp:LinkButton runat="server" CssClass="btn" ID="lnkSubmitCredit" style="display: none;" OnClick="lnkSubmitCredit_Click"><span>Submit Order</span></asp:LinkButton>
                    <%--<a href="#" class="btn" id="lnkSubmitCredit" style="display: none;"><span>Submit Order</span></a> --%>
                    <asp:LinkButton runat="server" CssClass="btn" id="lnkSubmit" OnClientClick="return forceCreditCard();blockUI();" OnClick="lnkSubmit_OnClick"><span>Submit Order</span></asp:LinkButton>
                    <asp:LinkButton runat="server" CssClass="btn" id="LinkButton2" OnClick="lnkCancel_OnClick"><span>Cancel</span></asp:LinkButton>
				</nav>
			</div>
		</div>
	</div>
</div>

<script type="text/javascript" language="javascript">
    
    (function () {
        var $,
          __indexOf = [].indexOf || function (item) { for (var i = 0, l = this.length; i < l; i++) { if (i in this && this[i] === item) return i; } return -1; };

        $ = jQuery;

        $.fn.validateCreditCard = function (callback, options) {
            var card, card_type, card_types, get_card_type, is_valid_length, is_valid_luhn, normalize, validate, validate_number, _i, _len, _ref, _ref1;
            card_types = [
              {
                  name: 'amex',
                  pattern: /^3[47]/,
                  valid_length: [15]
              }, {
                  name: 'diners_club_carte_blanche',
                  pattern: /^30[0-5]/,
                  valid_length: [14]
              }, {
                  name: 'diners_club_international',
                  pattern: /^36/,
                  valid_length: [14]
              }, {
                  name: 'jcb',
                  pattern: /^35(2[89]|[3-8][0-9])/,
                  valid_length: [16]
              }, {
                  name: 'laser',
                  pattern: /^(6304|670[69]|6771)/,
                  valid_length: [16, 17, 18, 19]
              }, {
                  name: 'visa_electron',
                  pattern: /^(4026|417500|4508|4844|491(3|7))/,
                  valid_length: [16]
              }, {
                  name: 'visa',
                  pattern: /^4/,
                  valid_length: [16]
              }, {
                  name: 'mastercard',
                  pattern: /(5[1-5]|222[1-9]|22[3-9]|2[3-6]|27[01]|2720)/,
                  valid_length: [16]
              }, {
                  name: 'maestro',
                  pattern: /^(5018|5020|5038|6304|6759|676[1-3])/,
                  valid_length: [12, 13, 14, 15, 16, 17, 18, 19]
              }, {
                  name: 'discover',
                  pattern: /^(6011|622(12[6-9]|1[3-9][0-9]|[2-8][0-9]{2}|9[0-1][0-9]|92[0-5]|64[4-9])|65)/,
                  valid_length: [16]
              }
            ];
            if (options == null) {
                options = {};
            }
            if ((_ref = options.accept) == null) {
                options.accept = (function () {
                    var _i, _len, _results;
                    _results = [];
                    for (_i = 0, _len = card_types.length; _i < _len; _i++) {
                        card = card_types[_i];
                        _results.push(card.name);
                    }
                    return _results;
                })();
            }
            _ref1 = options.accept;
            for (_i = 0, _len = _ref1.length; _i < _len; _i++) {
                card_type = _ref1[_i];
                if (__indexOf.call((function () {
                  var _j, _len1, _results;
                  _results = [];
                  for (_j = 0, _len1 = card_types.length; _j < _len1; _j++) {
                    card = card_types[_j];
                    _results.push(card.name);
                }
                  return _results;
                })(), card_type) < 0) {
                    throw "Credit card type '" + card_type + "' is not supported";
                }
            }
            get_card_type = function (number) {
                var _j, _len1, _ref2;
                _ref2 = (function () {
                    var _k, _len1, _ref2, _results;
                    _results = [];
                    for (_k = 0, _len1 = card_types.length; _k < _len1; _k++) {
                        card = card_types[_k];
                        if (_ref2 = card.name, __indexOf.call(options.accept, _ref2) >= 0) {
                            _results.push(card);
                        }
                    }
                    return _results;
                })();
                for (_j = 0, _len1 = _ref2.length; _j < _len1; _j++) {
                    card_type = _ref2[_j];
                    if (number.match(card_type.pattern)) {
                        return card_type;
                    }
                }
                return null;
            };
            is_valid_luhn = function (number) {
                var digit, n, sum, _j, _len1, _ref2;
                sum = 0;
                _ref2 = number.split('').reverse();
                for (n = _j = 0, _len1 = _ref2.length; _j < _len1; n = ++_j) {
                    digit = _ref2[n];
                    digit = +digit;
                    if (n % 2) {
                        digit *= 2;
                        if (digit < 10) {
                            sum += digit;
                        } else {
                            sum += digit - 9;
                        }
                    } else {
                        sum += digit;
                    }
                }
                return sum % 10 === 0;
            };
            is_valid_length = function (number, card_type) {
                var _ref2;
                return _ref2 = number.length, __indexOf.call(card_type.valid_length, _ref2) >= 0;
            };
            validate_number = function (number) {
                var length_valid, luhn_valid;
                card_type = get_card_type(number);
                luhn_valid = false;
                length_valid = false;
                if (card_type != null) {
                    luhn_valid = is_valid_luhn(number);
                    length_valid = is_valid_length(number, card_type);
                }
                return callback({
                    card_type: card_type,
                    luhn_valid: luhn_valid,
                    length_valid: length_valid
                });
            };
            validate = function () {
                var number;
                number = normalize($(this).val());
                return validate_number(number);
            };
            normalize = function (number) {
                return number.replace(/[ -]/g, '');
            };
            this.bind('input', function () {
                $(this).unbind('keyup');
                return validate.call(this);
            });
            this.bind('keyup', function () {
                return validate.call(this);
            });
            if (this.length !== 0) {
                validate.call(this);
            }
            return this;
        };

    }).call(this);

    $(function () {
        $('.po-pane-move').css('position', 'relative');

        $("#<%= lnkSubmitCredit.ClientID %>,#<%= lnkSubmitCreditTop.ClientID %>").click(function (e) {
            e.preventDefault();           
            _gaq.push(['_trackEvent', 'Conversions', 'Submit Order']);       
            window.setTimeout("window.location.href='" + $(this).attr('href') + "'", 100);
        });
        
        $("#<%= lnkSubmit.ClientID %>,#<%= lnkSubmitTop.ClientID %>").click(function (e) {
            _gaq.push(['_trackEvent', 'Conversions', 'Submit Order']);
        });

        $("#<%= lnkSubmitCredit.ClientID %>,#<%= lnkSubmitCreditTop.ClientID %>").click(function() {
            var test = '';
            blockUI();
            
            $('span.err').hide();

          /*  if ($('#cardName').val().length == 0) {
                $('#cardName').next('span.err').show();
                $('#cardName').focus();
                unblockUI();
                return false;
            }
*/
  if ($('#<%= cardFirstName.ClientID %>').val().length == 0) {
                $('#<%= cardFirstName.ClientID %>').next('span.err').show();
                $('#<%= cardFirstName.ClientID %>').focus();
                unblockUI();
                return false;
            }

  if ($('#<%= cardLastName.ClientID %>').val().length == 0) {
                $('#<%= cardLastName.ClientID %>').next('span.err').show();
                $('#<%= cardLastName.ClientID %>').focus();
                unblockUI();
                return false;
            }

            if ($('#<%= cardNum.ClientID %>').val().length == 0) {
                $('#<%= cardNum.ClientID %>').next('span.err').show();
                $('#<%= cardNum.ClientID %>').focus();
                unblockUI();
                return false;
            }  
            if ($('#<%= cardCSV.ClientID %>').val().length == 0) {
                $('#<%= cardCSV.ClientID %>').next('span.err').show();
                $('#<%= cardCSV.ClientID %>').focus();
                unblockUI();
                return false;
            }

            if ($('#<%= cardMonth.ClientID %>').val() == 0 || (parseInt($('#<%= cardMonth.ClientID %>').val()) < parseInt('<%= DateTime.Now.Month %>') && parseInt($('#<%= cardYear.ClientID %>').val()) == parseInt('<%= DateTime.Now.Year %>'))) {
               $('#<%= cardMonth.ClientID %>').siblings('.err').show();
                $('#<%= cardMonth.ClientID %>').focus();
                unblockUI();
                return false;
            }

            if ($('#<%= cardYear.ClientID %>').val().length == 0) {
                $('#<%= cardYear.ClientID %>').next('span.err').show();
                $('#<%= cardYear.ClientID %>').focus();
                unblockUI();
                return false;
            }

            $('#<%= cardNum.ClientID %>').validateCreditCard(function (result) {
                //var paymentTypeName = $("input[name='cardType[]']:checked").attr("id");
                var paymentTypeName = $('.cardType:checked').attr("id");
                var isDefaultPayment = $('#futureOrders').is(':checked');
                if (result.length_valid && result.luhn_valid) {

                    if ((result.card_type.name == "visa" && paymentTypeName == "cardVisa") ||
                    (result.card_type.name == "mastercard" && paymentTypeName == "cardMC") ||
                    (result.card_type.name == "amex" && paymentTypeName == "cardAMEX") ||
                    (result.card_type.name == "discover" && paymentTypeName == "cardDisc")) {
                         unblockUI();
  $('#<%= btnPostNew.ClientID %>').click();
                         return true;
                    } else {
                        $('#<%= cardNum.ClientID %>').next('span.err').html('Please enter a valid Card Type to match the Card Number');
                        $('#<%= cardNum.ClientID %>').next('span.err').show();
                        $('#<%= cardNum.ClientID %>').focus();
                        unblockUI();
                        return false;
                    }
                } else {
                    $('#<%= cardNum.ClientID %>').next('span.err').html('Please enter a valid Credit Card Number');
                    $('#<%= cardNum.ClientID %>').next('span.err').show();
                    $('#<%= cardNum.ClientID %>').focus();
                    unblockUI();
                    return false;
                }
            }, { accept: ['visa', 'mastercard', 'discover', 'amex'] });
        });

        $('#emailYes').click(function() {
            $('#emailNo').removeAttr('checked').next('label').removeClass('checked');
            $(this).attr('checked', 'checked').next('label').addClass('checked');
        });
        
        $('#emailNo').click(function () {
            $('#emailYes').removeAttr('checked').next('label').removeClass('checked');
            $(this).attr('checked', 'checked').next('label').addClass('checked');
        });

        $('#<%= cardNum.ClientID %>, #<%= cardCSV.ClientID %>').keypress(function () {
            return isNumberKey(event);
        });

        $('.form-el.radio label').click(function () {
            if (!$(this).hasClass('checked')) {
                var radioName = $(this).prev().attr('name');
                $('input[name="' + radioName + '"]:checked').next().removeClass('checked');
                $(this).addClass('checked');
            }
        });

        $('#orderOnly').click(function () {
            $('#futureOrders').removeAttr('checked').next('label').removeClass('checked');
            $(this).attr('checked', 'checked').next('label').addClass('checked');
        });

        $('#futureOrders').click(function () {
            $('#orderOnly').removeAttr('checked').next('label').removeClass('checked');
            $(this).attr('checked', 'checked').next('label').addClass('checked');
        });
        
        $('.po-panes select').selectBox();

    });
    
    function blockUI() {
        $.blockUI({
            css: {
                border: 'none',
                padding: '15px',
                backgroundColor: '#000',
                '-webkit-border-radius': '10px',
                '-moz-border-radius': '10px',
                opacity: .5,
                color: '#fff'
            }
        });
    }

    function unblockUI() {
        $.unblockUI();
    }
    function loadOverviewMenu() {
    }
    function updateCart() {
    }
    function toggleNewCard(obj) {
        if ($(obj).hasClass('checked')) {
            $('#savedPayment').removeAttr('checked');
            $(obj).removeClass('checked');
            $('.new-card').slideDown();
            $('#<%= lnkSubmit.ClientID %>, #<%= lnkSubmitTop.ClientID %>').hide();
            $('#<%= lnkSubmitCredit.ClientID %>, #<%= lnkSubmitCreditTop.ClientID %>').show();
        } else {
            $('#savedPayment').attr('checked', 'checked');
            $(obj).addClass('checked');
            $('.new-card').slideUp();
            $('#<%= lnkSubmit.ClientID %>, #<%= lnkSubmitTop.ClientID %>').show();
            $('#<%= lnkSubmitCredit.ClientID %>, #<%= lnkSubmitCreditTop.ClientID %>').hide();
        }
    }

    function forceCreditCard(obj) {
        if ('<%= AccountInfo.Account.PaymentTypeDescription %>' == "COLLECT PAYMENT") {
            alert('You must enter credit card information for this order. COLLECT PAYMENT defaults can not be processed');
            //if ($(obj).hasClass('checked')) {
                $('#savedPayment').removeAttr('checked');
                $('#lblSavedPayment').removeClass('checked');
                $('.new-card').slideDown();
                $('#<%= lnkSubmit.ClientID %>, #<%= lnkSubmitTop.ClientID %>').hide();
             $('#<%= lnkSubmitCredit.ClientID %>, #<%= lnkSubmitCreditTop.ClientID %>').show();
            return false;
        }
        return true;
    }
    
    function isNumberKey(evt) {
        var charCode = (evt.which) ? evt.which : event.keyCode;
        if (charCode != 46 && charCode > 31
            && (charCode < 48 || charCode > 57))
            return false;

        return true;
    }
</script>