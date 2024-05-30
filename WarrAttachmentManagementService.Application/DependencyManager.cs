using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using WarrAttachmentManagementService.Application.Interfaces.Services;
using WarrAttachmentManagementService.Application.LineItemAttachment;
using WarrAttachmentManagementService.Application.LineItemAttachmentType;
using WarrAttachmentManagementService.Application.RepairOrderAttachment;
using WarrAttachmentManagementService.Application.Validators;
using WarrAttachmentManagementService.Application.MultiPartUploads;
using WarrAttachmentManagementService.Application.RepairOrderTestFile;
using WarrAttachmentManagementService.Application.S3File;

namespace WarrAttachmentManagementService.Application;

public static class DependencyManager
{
    public static IServiceCollection AddApplicationServices(
            this IServiceCollection services,
            IConfiguration configuration)
    {
        //services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddScoped<IValidator<RepairOrderAttachmentCreateRequest>, RepairOrderAttachmentValidator>();
        services.AddScoped<IValidator<RepairOrderAttachmentsCreateRequest>, RepairOrderAttachmentsValidator>();
        services.AddScoped<IValidator<LineItemAttachmentCreateRequest>, LineItemAttachmentValidator>();
        services.AddScoped<IValidator<LineItemAttachmentsCreateRequest>, LineItemAttachmentsValidator>();
        services.AddScoped<IValidator<InitiateMultipartUploadRequest>, InitiateMultipartUploadRequestValidator>();
        services.AddScoped<IValidator<UploadFilePartRequest>, UploadFilePartRequestValidator>();
        services.AddScoped<IValidator<CompleteMultipartUploadRequest>, CompleteMultipartUploadRequestValidator>();
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddScoped<ILineItemAttachmentService, LineItemAttachmentService>();
        services.AddScoped<ILineItemAttachmentTypeService, LineItemAttachmentTypeService>();
        services.AddScoped<IRepairOrderAttachmentService, RepairOrderAttachmentService>();
        services.AddScoped<IMultiPartUploadService, MultiPartUploadService>();
        services.AddScoped<IRepairOrderTestFileService,RepairOrderTestFileService>();
        services.AddScoped<IS3FileService, S3FileService>();

        return services;
    }
}
