namespace DevOpsCentre.Bff.Azure

module Team =

  let private skele = [
    "bank_directory-db"
    "bank_directory-sf"
    "bank_directory-tf"
    "chaps_metadata-db"
    "chaps_metadata-sf"
    "chaps_monitoring_and_alerting"
    "chaps_database"
    "returns_chaps-sf"
    "end2end-team-skeletorus_regression-tests"
    "failedmessages_chaps_dsl-sf"
    "fi_initiation_chaps-sf"
    "manual-workaround-chapsmt202-addpartyidentifier"
    "manual-workaround-chapsmt202-updatepartyidentifier"
    "manual-workaround-collectiveapproval-setupactionconfiguration"
    "pain_chaps-sf"
    "payments_assessment"
    "payments_chaps_native"
    "payments_chaps_v2"
    "payments_chaps"
    "payments_validation"
    "privateproxy"
    "repairs_chaps"
    "returns_chaps_dsl-sf"
    "returns_chaps-db"
    "returns_chaps-sf"
    "swift_chaps"
    "swift_database"
    "transaction_notification_alerting"
    "transaction_notifications_processor-sf"
    "transaction_notifications-sf"
    "transaction_notification_consoles"
  ]

  let private narwhal = [
    "azure_servicebus_entity_destroyer"
    "bacs_alerts"
    "service_fabric_test_runner"
    "bacs"
    "bacs_message_batcher-fn"
    "bacs_recalls-fn"
    "bacs_scheme_test-fn"
    "bacs"
    "bacs_webhooks-fn"
    "fi_bacs-sf"
    "hsm"
    "manual-workaround-bacs-sortcode-mapping"
    "paymentscalendar"
    "swift_bacs_database"
    "swift_bacs"
    "transformer"
  ]

  let private gecko = [
    "bic_manager"
    "compliance_screening"
    // "customer_account_products"
    // "customer_accounts_adapters"
    // "customer_accounts"
    // "customers"
    "gpi_tracking-sf"
    "institutionidentification_dsl-sf"
    "institutions"
    "organisations"
    "products"
    "swift_auditing"
    "swift_connection_manager-db"
    "swift_connection_manager-sf"
    "swift_connector_v2"
    "swift_messaging"
    "swift_processor"
  ]
  
  let allRepos = 
    [ yield! skele; yield! narwhal; yield! gecko ] // combine all lists
    |> List.map (fun a -> a,a) // map them into key pairs
    |> Map.ofList // create map from that