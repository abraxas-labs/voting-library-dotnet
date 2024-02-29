// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using FluentAssertions;
using Voting.Lib.DmDoc.Models;
using Voting.Lib.DmDoc.Models.Internal;
using Voting.Lib.DmDoc.Serialization.Json;
using Xunit;

namespace Voting.Lib.DmDoc.Test.Serialization.Json;

public class DmDocJsonSerializerTest
{
    [Fact]
    public void SerializeShouldWork()
    {
        var testData = new CreateDraftRequest
        {
            FinishEditing = new FinishEditingData(),
            Async = true,
            Data = new List<CreateDraftData>(),
            CallbackActions = new[] { CallbackAction.CreateError, CallbackAction.FinishEditing, CallbackAction.FinishEditingError },
            CallbackUrl = "https://example.com",
            CallbackTimeout = 1,
            CallbackRetryPolicy = new CallbackRetryData
            {
                MaxRetries = 5,
                RetryInterval = 1000,
                RetryType = 2,
            },
            TemplateId = 234,
        };

        var serialized = DmDocJsonSerializer.Serialize(testData);
        serialized.Should().Be("{\"template_id\":234,\"data\":[],\"async\":true,\"callback_url\":\"https://example.com\","
            + "\"callback_actions\":[\"create_error\",\"finish_editing\",\"finish_editing_error\"],"
            + "\"finish_editing\":{\"distribution\":\"local_pdf\"},"
            + "\"callback_retry_policy\":{\"retry_type\":2,\"max_retries\":5,\"retry_interval\":1000},\"callback_timeout\":1,\"fail_async_job_on_callback_failure\":false}");
    }

    [Fact]
    public void DeserializeShouldWork()
    {
        var jsonString = "{\"action\":\"finish_editing\",\"object_id\":999,\"object_class\":\"Draft\","
            + "\"message\":\"test\",\"data\":{\"print_job_id\":234}}";

        var deserialized = DmDocJsonSerializer.Deserialize<CallbackData>(jsonString);

        var expected = new CallbackData
        {
            Action = CallbackAction.FinishEditing,
            Data = new CallbackCustomData { PrintJobId = 234 },
            Message = "test",
            ObjectClass = "Draft",
            ObjectId = 999,
        };
        deserialized.Should().BeEquivalentTo(expected);
    }
}
