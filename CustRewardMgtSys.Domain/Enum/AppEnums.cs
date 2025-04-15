namespace CustRewardMgtSys.Domain.Enum
{
    public enum INCIDENT_CATEGORY_CAT_2 : byte
    {
        THEFT = 1,
        ARREST,
        SUSPECTED_IC,
        ATTACK,
        FIRE
    }

    public enum INCIDENT_CATEGORY : byte
    {
        PIPELINE_VANDALISM = 1,
        ILLEGAL_CONNECTION,
        ILLEGAL_REFINERY,
        ILLEGAL_VESSELS,
        COMMUNITY_ISSUES,
        THEFT,
        ARREST,
        WELL_HEAD,
        LOADING_POINT,
        SPILL_OR_LEAK,
        AVERSION,
        GENERAL,
        SUSPECTED_IC,
        ATTACK,
        FIRE,
        ILLEGAL_VESSEL,
        VESSEL_TRACKING,
        ILLEGAL_TRUCKING,
        GSIA_OPERATION,
        DOCUMENT_VALIDATION,
        COMMUNITY_ENGAGEMENT,

        OTHERS = 100
    }

    public enum PRIORITY : byte
    {
        LOW = 1,
        MEDIUM,
        HIGH,
        CRITICAL
    }

    public enum ACTION_TYPE : byte
    {
        REMOVED = 1,
        DESTROYED,
        CONFISCATED
    }
    public enum PROGRESS_STATUS : byte
    {
        NEW = 1,
        WAITING,
        REPLIED,
        IN_PROGRESS,
        RESOLVED,
        UN_RESOLVED,
        ALL_STATUS
    }

    public enum IMPORTANCE_STATUS : byte
    {
        NORMAL = 1,
        NOTABLE,
    }
    public enum CURRENT_STATUS : byte
    {
        CURRENT = 1,
        PREVIOUS,
    }

    public enum ACCESS_STATUS : byte
    {
        ENABLED = 1,
        DISABLE,
    }
    public enum GENDER : byte
    {
        MALE = 1,
        FEMALE,
    }

    public enum REPORT_SEARCH_BY_COLUMN : byte
    {
        REPORT_PERIOD = 1,
        REPORT_NUMBER,
        REPORT_DATE
    }

    public enum REPORT_ITEM_SEARCH_BY_COLUMN : byte
    {
        CORRIDOR_ID = 1,
        PIPELINE,
        SOURCE,
        COMMUNITY,
        PROGRESS_STATUS,
        TRACKING_ID,
        ZONE_OR_SEGMENT,
        INCIDENT_OWNER
    }

    public enum INCIDENT_FILE_TYPE : byte
    {
        INCIDENTS_ZONES_OR_SEGMENTS_HANDLING = 1,
        NEW_INCIDENTS_ENTERING,
        INCIDENTS_PROGRESS_UPDATE,
        INCIDENTS_PORTAL_MISSING_INCIDENT_TAG,
        INFOPOOL_MISSING_INCIDENTS,
        RECOVERY_FACTOR_UPLOAD
    }

    public enum DATE_COLUMN_NAME : byte
    {
        INCIDENT_DATE = 1,
        REPORT_DATE_AND_TIME,
        RESOLVED_DATE,
    }

    public enum REPORTING_TYPE : byte
    {
        INTITIAL = 1,
        RESPONSE,
        ALL
    }

    public enum TABLE_STATUS : byte
    {
        PSC = 1,
        Non_PSC
    }
    public enum USER_TYPE : byte
    {
        ADMIN = 1,
        PAINT_BUYER
    }
    public enum PIN_STATUS : byte
    {
        NEW = 1,
        DISPATCHED,
        USED,
        REWARDED_OF
    }
    public enum RECONCILIATION_TYPE : byte
    {
        CLEANED = 1,
        DUPLICATE_MAIN,
        DUPLICATE_REPEATATION,
        ASSIGNED_TO_SECURITY,
        YET_TO_VERIFY_WHY_MISSING_FROM_PORTAL,//Whether DUPLICATE_MAIN or DUPLICATE_REPEATATION or ASSIGNED_TO_SECURITY
        DUPLICATE_MAIN_ASSIGNED_TO_SECURITY,
        NO_LONGER_AVAILABLE_ON_THE_PORTAL
    }


    public enum DISCREPANCY_STATUS : byte
    {
        MISSING_FROM_PORTAL = 1,
        MISSING_FROM_INFO_POOL,
        OTHERS = 20
    }
}