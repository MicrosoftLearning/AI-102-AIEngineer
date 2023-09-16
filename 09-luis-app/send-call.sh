curl -X POST "<ENDPOINT_URL>" \
-H "Ocp-Apim-Subscription-Key: <YOUR_KEY>" \
-H "Apim-Request-Id: <REQUEST_ID>" \
-H "Content-Type: application/json" \
-d "{\"kind\":\"Conversation\",\"analysisInput\":{\"conversationItem\":{\"id\":\"1\",\"text\":\"What's the time in Sydney\",\"modality\":\"text\",\"language\":\"EN\",\"participantId\":\"1\"}},\"parameters\":{\"projectName\":\"Clock\",\"verbose\":true,\"deploymentName\":\"production\",\"stringIndexType\":\"TextElement_V8\"}}"