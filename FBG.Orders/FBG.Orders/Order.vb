Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks

Namespace FBG_Order
    Public Class Item
        Public Property line_no As String
        Public Property item As String
        Public Property lot_number As Object
        Public Property quantity As Integer
        Public Property price As String
        Public Property discount As String
        Public Property addl_discount As String
        Public Property extended_amount As String
        Public Property tax As String
        Public Property shipping_surcharge As String
        Public Property line_item_id As String
        Public Property line_comment As String
        Public Property Description As String
        Public Property alt_sku As Object
        Public Property filtered_sku As String
        Public Property line_location As String
    End Class

    Public Class Item2
        Public Property item As String
        Public Property quantity As String
        Public Property carton_code As Object
        Public Property carton_num As Object
        Public Property box_length_in As Object
        Public Property box_width_in As Object
        Public Property box_height_in As Object
        Public Property package_weight_lbs As Object
        Public Property lot_code As Object
        Public Property custom_1 As Object
    End Class

    Public Class Shipment
        Public Property carrier As String
        Public Property method As String
        Public Property tracking_no As String
        Public Property tracking_no_secondary As Object
        Public Property sscc_barcode As Object
        Public Property bill_of_lading As Object
        Public Property total_cost As String
        Public Property package_weight_lbs As Object
        Public Property dim_weight_lbs As Object
        Public Property zone As Object
        Public Property delivery_surcharge_type As Object
        Public Property whs_location As Object
        Public Property box_length_in As Object
        Public Property box_width_in As Object
        Public Property box_height_in As Object
        'Public Property date As DateTime
        Public Property tracking_url As String
        Public Property custom_1 As Object
        Public Property custom_2 As Object
        Public Property custom_3 As Object
        Public Property items As List(Of Item2)
    End Class

    Public Class Response
        Public Property created_date_time As DateTime
        Public Property updated_date_time As DateTime
        Public Property record_no As String
        Public Property version As Object
        Public Property format As Object
        Public Property cust_ref As String
        Public Property cust_po_no As String
        Public Property po_number As String
        Public Property carrier As Object
        Public Property ship_code As String
        Public Property ship_code_description As Object
        Public Property working_ship_code As Object
        Public Property cust_company As String
        Public Property cust_title As Object
        Public Property cust_first_name As String
        Public Property cust_last_name As String
        Public Property cust_address_1 As String
        Public Property cust_address_2 As String
        Public Property cust_address_3 As Object
        Public Property cust_city As String
        Public Property cust_state As String
        Public Property cust_zip As String
        Public Property cust_country As String
        Public Property cust_phone As String
        Public Property cust_e_mail As String
        Public Property ship_company As String
        Public Property ship_title As Object
        Public Property ship_first_name As String
        Public Property ship_last_name As String
        Public Property ship_address_1 As String
        Public Property ship_address_2 As String
        Public Property ship_address_3 As Object
        Public Property ship_city As String
        Public Property ship_state As String
        Public Property ship_zip As String
        Public Property ship_country As String
        Public Property ship_phone As String
        Public Property ship_e_mail As String
        Public Property ship_address_type As Object
        Public Property special_services As Object
        Public Property customer_id As String
        Public Property order_date As String
        Public Property sub_total As String
        Public Property order_discount As String
        Public Property sales_tax As String
        Public Property shipping_handling As String
        Public Property grand_total As String
        Public Property balance As String
        Public Property currency_code As String
        Public Property credit_card_no As Object
        Public Property expiration_date As Object
        Public Property pay_type As String
        Public Property tax_exempt_sw As Object
        Public Property installment_program As Object
        Public Property media_week As Object
        Public Property order_source As String
        Public Property promo_code As Object
        Public Property ani_phone As Object
        Public Property vendor_phone As Object
        Public Property check_account_no As Object
        Public Property check_type As Object
        Public Property check_no As Object
        Public Property check_bank_id As Object
        Public Property check_cust_bank As Object
        Public Property check_cust_id_num As Object
        Public Property check_cust_id_state As Object
        Public Property check_cust_id_mm As Object
        Public Property check_cust_id_dd As Object
        Public Property check_cust_id_yy As Object
        Public Property check_cust_id_type As Object
        Public Property location As Object
        Public Property shipping_instructions As String
        Public Property ship_account_no As Object
        Public Property ship_account_zip As Object
        Public Property pre_auth_code As Object
        Public Property pre_auth_amt As String
        Public Property pre_auth_id As Object
        Public Property cvv As Object
        Public Property custom_field_1 As String
        Public Property custom_field_2 As String
        Public Property custom_field_3 As String
        Public Property custom_field_4 As Object
        Public Property custom_field_5 As Object
        Public Property gift_card_sw As Object
        Public Property token_sw As Object
        Public Property cass_code_ship As String
        Public Property cass_error_ship As Object
        Public Property cass_code_cust As Object
        Public Property cass_error_cust As Object
        Public Property cass_date As Object
        Public Property ncoa_code_ship As Object
        Public Property ncoa_code_cust As Object
        Public Property ncoa_date As Object
        Public Property error_code As Object
        Public Property error_msg As Object
        Public Property ifraud_error_code As Object
        Public Property xfraud_error_code As Object
        Public Property credit_error_code As Object
        Public Property resubmit_date As Object
        Public Property black_list As Object
        Public Property credit_score As Object
        Public Property fraud_score As Object
        Public Property load_override_sw As Object
        Public Property fail_action As Object
        Public Property action_dt As Object
        Public Property filename As Object
        Public Property call_queue As Object
        Public Property clerk_disposition As Object
        Public Property clerk_disp_dt As Object
        Public Property rep_disposition As Object
        Public Property rep_disp_dt As Object
        Public Property duplicate_sw As Object
        Public Property weight As String
        Public Property org_file_no As Object
        Public Property gift_message As Object
        Public Property gift_wrap As Object
        Public Property delete_date As Object
        Public Property routing_sw As String
        Public Property sent_to_region As Object
        Public Property accepted_by_region As Object
        Public Property regional_center As Object
        Public Property regional_order_no As Object
        Public Property regional_ship_date As Object
        Public Property regional_retry_sw As String
        Public Property regional_error As Object
        Public Property regional_attempts As String
        Public Property first_attempt As Object
        Public Property cancel_date As Object
        Public Property cc_last_four As Object
        Public Property expected_delivery_date As String
        Public Property requested_ship_date As String
        Public Property delivered_to_wms_date As DateTime?
        Public Property error_reason As Object
        Public Property mark_in_progress_date As DateTime
        Public Property extra_system_date_sent As Object
        Public Property sending_canceled As String
        Public Property shipping_pickup_canceled As String
        Public Property on_hold As String
        Public Property num_failed_sends As String
        Public Property inventory_warehouse_pk As String
        Public Property latest_ship_date As Object
        Public Property log_documents_in As Object
        Public Property log_documents_out As Object
        Public Property mark_in_progress_log As Object
        Public Property orig_ship_code As String
        Public Property orig_carrier As Object
        Public Property order_status As String
        Public Property items As List(Of Item)
        Public Property shipments As List(Of Shipment)
    End Class

    Public Class Order
        Public Property success_code As Boolean
        Public Property response As List(Of Response)
    End Class
End Namespace