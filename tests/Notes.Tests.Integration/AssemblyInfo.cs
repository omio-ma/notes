// Disable test parallelization to avoid concurrency issues with shared resources,
// such as the test database, which can cause flaky or conflicting test runs.
[assembly: CollectionBehavior(DisableTestParallelization = true)]
